using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using CefSharp;
using CefSharp.OffScreen;

using BangumiX.Properties;
using System.Windows.Media.Imaging;

namespace BangumiX.Common
{
    public class HttpHelper
    {
        public static readonly HttpClient APIclient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/")
        };
        public static readonly HttpClient TokenClient = new HttpClient()
        {
            BaseAddress = new Uri("http://47.101.195.180:5000/")
        };
        public static readonly HttpClient HTMLclient = new HttpClient()
        {
            BaseAddress = new Uri("https://bangumi.tv/")
        };
        public static string host = HTMLclient.BaseAddress.ToString();

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

        public static async Task<HttpResult> CheckLogin()
        {
            HttpResult check_login_result = new HttpResult();
            if (Settings.Default.NeverAsk == true)
            {
                check_login_result.Status = 1;
                return check_login_result;
            }
            if (Settings.Default.AccessToken != String.Empty)
            {
                var time_remain = (int)(DateTime.Now - Settings.Default.TokenTime).TotalSeconds;
                if (time_remain < Settings.Default.Expire / 2)
                {
                    APIclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Settings.Default.TokenType, Settings.Default.AccessToken);
                    check_login_result.Status = 1;
                    return check_login_result;
                }
                else
                {
                    var refresh_token_result = await StartRefreshToken();
                    if (refresh_token_result.Status == 1)
                    {
                        check_login_result.Status = 1;
                        return check_login_result;
                    }
                    else Console.WriteLine("Refresh Failed");
                }
            }
            return check_login_result;
        }

        public class CaptchaSrcResult : HttpResult
        {
            public BitmapImage CaptchaSrc { get; set; }
        }
        public class LoginResult : HttpResult
        {
            public Model.Token Token { get; set; }
        }
        public class StartLogin
        {
            public ChromiumWebBrowser browser;
            public string login_uri;
            public Model.Login login;
            public CaptchaSrcResult captcha_src_result;
            public LoginResult login_result;
            public StartLogin()
            {
                Cef.Initialize(new CefSettings());
                login_uri = String.Format("https://bgm.tv/oauth/authorize?client_id={0}&response_type=code", Settings.Default.ClientID);
                browser = new ChromiumWebBrowser(browserSettings: new BrowserSettings());
                while (!browser.IsBrowserInitialized)
                {
                    System.Threading.Thread.Sleep(100);
                }
                
                login = new Model.Login();
                captcha_src_result = new CaptchaSrcResult();
                login_result = new LoginResult();
            }
            public void Shutdown()
            {
                Cef.Shutdown();
            }
            public async Task GetCaptchaSrc()
            {
                await LoadPageAsync(browser, login_uri);

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

                string captcha_string = string.Empty;
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
                            captcha_string = response.Result.ToString().Substring(22);
                        }
                    }
                });

                byte[] binaryData = Convert.FromBase64String(captcha_string);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new System.IO.MemoryStream(binaryData);
                bi.EndInit();
                captcha_src_result.CaptchaSrc = bi;

                captcha_src_result.Status = 1;
                return;
            }

            public async Task Start()
            {
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
                            login_result.Token = JsonConvert.DeserializeObject<Model.Token>(response.Result.ToString());
                            login_result.Token.token_time = DateTime.Now;
                        }
                    }
                });

                login_result.Status = 1;

                return;
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

        public class RefreshTokenResult : HttpResult
        {
            public Model.Token Token { get; set; }
        }
        public static async Task<RefreshTokenResult> StartRefreshToken()
        {
            RefreshTokenResult refresh_token_result = new RefreshTokenResult();
            try
            {
                string url = String.Format("callback?refresh_token={0}", Settings.Default.RefreshToken);
                HttpResponseMessage response = await TokenClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                refresh_token_result.Token = JsonConvert.DeserializeObject<Model.Token>(response_body);
                refresh_token_result.Status = 1;
                return refresh_token_result;
            }
            catch (HttpRequestException e)
            {
                refresh_token_result.Status = -1;
                refresh_token_result.ErrorMessage = e.Message;
                return refresh_token_result;
            }
        }

        public class WatchingResult : HttpResult
        {
            public List<Model.Collection> Watching { get; set; }
        }
        public static async Task<WatchingResult> GetWatching(uint id, string cat = "watching")
        {
            WatchingResult watching_result = new WatchingResult();
            try
            {
                string url = String.Format("user/{0}/collection?app_id={1}&cat={2}", id, Settings.Default.ClientID, cat);
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                watching_result.Watching = JsonConvert.DeserializeObject<List<Model.Collection>>(response_body);
                watching_result.Status = 1;
                return watching_result;
            }
            catch (HttpRequestException e)
            {
                watching_result.Status = -1;
                watching_result.ErrorMessage = e.Message;
                return watching_result;
            }
        }

        public class RecentCollectionResult : HttpResult
        {
            public List<Model.CollectsWrapper> CollectWrapper { get; set; }
        }
        public static async Task<RecentCollectionResult> GetRecentCollection(uint id, string subject_type = "anime")
        {
            RecentCollectionResult collection_result = new RecentCollectionResult();
            try
            {
                string url = String.Format("user/{0}/collections/{1}?app_id={2}", id, subject_type, Settings.Default.ClientID);
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                collection_result.CollectWrapper = JsonConvert.DeserializeObject<List<Model.CollectsWrapper>>(response_body);
                collection_result.Status = 1;
                return collection_result;
            }
            catch (HttpRequestException e)
            {
                collection_result.Status = -1;
                collection_result.ErrorMessage = e.Message;
                return collection_result;
            }
        }

        public class SubjectResult : HttpResult
        {
            public dynamic Subject { get; set; }
            public SubjectResult()
            {
                Subject = new Model.SubjectSmall();
            }
        }
        public static async Task<SubjectResult> GetSubject(uint id, string response_group = "large")
        {
            SubjectResult subject_result = new SubjectResult();
            try
            {
                string url = String.Format("subject/{0}?responseGroup={1}", id, response_group);
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                if (response_group == "small") subject_result.Subject = JsonConvert.DeserializeObject<Model.SubjectSmall>(response_body);
                else if (response_group == "medium") subject_result.Subject = JsonConvert.DeserializeObject<Model.SubjectLarge>(response_body);
                else if (response_group == "large") subject_result.Subject = JsonConvert.DeserializeObject<Model.SubjectLarge>(response_body);
                subject_result.Status = 1;
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
