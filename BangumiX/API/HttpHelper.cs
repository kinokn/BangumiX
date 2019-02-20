using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using HtmlAgilityPack;
using CefSharp;
using CefSharp.OffScreen;
//using CefSharp.Wpf;

using BangumiX.Properties;


namespace BangumiX.API
{
    class HttpHelper
    {
        private static readonly HttpClient APIclient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/")
        };

        public class HttpResult
        {
            public int Status { get; set; }
            public string ErrorMessage { get; set; }
            public HttpResult()
            {
                Status = 0;
                ErrorMessage = String.Empty;
            }
        }

        public class CaptchaSrcResult : HttpResult
        {
            public string CaptchaSrc { get; set; }
        }
        public class LoginResult : HttpResult
        {
            public Token Token { get; set; }
            public LoginResult()
            {
                Token = new Token();
            }
        }
        public static class StartLogin
        {
            public static ChromiumWebBrowser browser;
            public static string LoginUri;
            public static async Task Initialize()
            {
                Cef.Initialize(new CefSettings()
                {
                    PersistSessionCookies = true
                });
                LoginUri = String.Format("https://bgm.tv/oauth/authorize?client_id={0}&response_type=code", Settings.Default.ClientID);
                browser = new ChromiumWebBrowser(LoginUri, new BrowserSettings());
                await LoadPageAsync(browser);
            }
            public static async Task<CaptchaSrcResult> GetCaptchaSrc()
            {
                CaptchaSrcResult captcha_src_result = new CaptchaSrcResult();
                string script = @"(function() {
                                    var email = document.getElementById('email');
                                    var password = document.getElementById('password');
                                    var ev = new Event('input');
                                    email.value = 1;
                                    password.value = 1;
                                    email.dispatchEvent(ev);
                                })();";

                browser.ExecuteScriptAsync(script);
                System.Threading.Thread.Sleep(1000);

                script = @"(function() {var img = document.getElementById('captcha_img_code');
                                var canvas = document.createElement('canvas');
                                canvas.width = img.width;
                                canvas.height = img.height;
                                var ctx = canvas.getContext('2d');
                                ctx.drawImage(img, 0, 0, img.width, img.height);
                                var ext = img.src.substring(img.src.lastIndexOf('.') + 1).toLowerCase();
                                var dataURL = canvas.toDataURL('image/' + ext);
                                return dataURL;
                            })();";

                var task = browser.EvaluateScriptAsync(script);
                await task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            captcha_src_result.CaptchaSrc = response.Result.ToString().Substring(22); // Remove "data:image/png;base64,"
                        }
                    }
                });
                captcha_src_result.Status = 1;
                return captcha_src_result;
            }
            public static async Task<LoginResult> Start(Login login)
            {
                LoginResult login_result = new LoginResult();
                string script = String.Format(@"(function() {{ var email = document.getElementById('email')
                                                email.value = '{0}';
                                                document.getElementById('password').value = '{1}';
                                                var ev = new Event('input');
                                                email.dispatchEvent(ev);
                                                document.getElementById('captcha').value = '{2}';
                                                document.getElementsByClassName('inputBtn')[0].click();}})();", login.Email, login.Password, login.Captcha);
                browser.ExecuteScriptAsync(script);
                await LoadPageAsync(browser);

                script = @"document.getElementsByClassName('btnPink large')[0].click();";
                browser.ExecuteScriptAsync(script);
                await LoadPageAsync(browser);

                script = "(function () { return document.documentElement.innerText; })();";
                var task = browser.EvaluateScriptAsync(script);
                await task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success && response.Result != null)
                        {
                            login_result.Token = JsonConvert.DeserializeObject<Token>(response.Result.ToString());
                        }
                    }
                });

                login_result.Status = 1;

                return login_result;
            }

            public static Task LoadPageAsync(IWebBrowser browser, string address = null)
            {
                var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

                EventHandler<LoadingStateChangedEventArgs> handler = null;
                handler = (sender, args) =>
                {
                    if (!args.IsLoading)
                    {
                        browser.LoadingStateChanged -= handler;
                        tcs.TrySetResult(true);
                    }
                };

                browser.LoadingStateChanged += handler;

                if (!string.IsNullOrEmpty(address))
                {
                    browser.Load(address);
                }
                return tcs.Task;
            }
        }

        public class GetSubjectResult : HttpResult
        {
            public dynamic Subject { get; set; }
            public GetSubjectResult()
            {
                Subject = new SubjectSmall();
            }
        }
        public static async Task<GetSubjectResult> GetSubject(int id, int response_group = 0)
        {
            GetSubjectResult subject_result = new GetSubjectResult();
            try
            {
                string url = String.Format("subject/{0}?responseGroup=", id);
                switch (response_group)
                {
                    case 0:
                        url += "small";
                        break;
                    case 1:
                        url += "medium";
                        break;
                    case 2:
                        url += "large";
                        break;
                }
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                subject_result.Status = 1;
                switch (response_group)
                {
                    case 0:
                        subject_result.Subject = JsonConvert.DeserializeObject<SubjectSmall>(response_body);
                        break;
                    case 1:
                        subject_result.Subject = JsonConvert.DeserializeObject<SubjectLarge>(response_body);
                        break;
                    case 2:
                        subject_result.Subject = JsonConvert.DeserializeObject<SubjectLarge>(response_body);
                        break;
                }
                return subject_result;
            }
            catch (HttpRequestException e)
            {
                subject_result.Status = -1;
                subject_result.ErrorMessage = e.Message;
                return subject_result;
            }
        }
    }
}
