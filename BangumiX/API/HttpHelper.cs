using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using HtmlAgilityPack;

using BangumiX.Properties;


namespace BangumiX.API
{
    class HttpHelper
    {
        private static readonly HttpClient JSONclient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/")
        };
        public static CookieContainer Cookies = new CookieContainer();
        public static HttpClientHandler HttpClientHandler = new HttpClientHandler() { CookieContainer = Cookies };
        private static readonly HttpClient HTMLclient = new HttpClient(HttpClientHandler)
        {
            BaseAddress = new Uri("https://bangumi.tv/")
        };
        public static string host = HTMLclient.BaseAddress.AbsoluteUri;
        public static void ClientInitialize()
        {
            host = host.Substring(host.Length - 1);
            JSONclient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (compatible; AcmeInc/1.0");
            JSONclient.DefaultRequestHeaders.Add("Accept", "text/html");
            if (Settings.Default.Cookies != null) Cookies.Add(Settings.Default.Cookies);
        }

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

        public class CheckLoginResult : HttpResult
        {
            public Login Login { get; set; }
            public CheckLoginResult()
            {
                Login = new Login();
            }
        }
        public static async Task<CheckLoginResult> CheckLogin()
        {
            CheckLoginResult login_result = new CheckLoginResult();
            try
            {
                HttpResponseMessage response = await HTMLclient.GetAsync(String.Empty);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response_body);
                var form = htmlDoc.DocumentNode.SelectSingleNode("//form[@id=\"loginForm\"]");
                if (form == null)
                {
                    login_result.Status = 1;
                }
                else
                {
                    login_result.Status = 0;
                    login_result.Login.FormHash = form.SelectSingleNode("//input[@name=\"formhash\"]").Attributes["value"].Value;
                    login_result.Login.CookieTime = form.SelectSingleNode("//input[@name=\"cookietime\"]").Attributes["value"].Value;
                    if (form.SelectSingleNode("//dt[@id=\"rechaptcha_form\"]").Attributes["style"].Value != "display:none")
                    {
                        login_result.Login.ChaptchaSrc = host + form.SelectSingleNode("//img[@id=\"captcha_img_code\"]").Attributes["src"].Value;
                    }
                    else
                    {
                        login_result.Login.Chaptcha = "不需要填写";
                    }
                }
                return login_result;
            }
            catch (HttpRequestException e)
            {
                login_result.Status = -1;
                login_result.ErrorMessage = e.Message;
                return login_result;
            }
        }

        public static async Task<HttpResult> StartLogin(Login login)
        {
            HttpResult login_result = new HttpResult();
            var parameters = new Dictionary<string, string>
            {
                { "formhash", login.FormHash },
                { "cookietime", login.CookieTime },
                { "email", login.Email },
                { "password", login.Password },
                { "captcha_challenge_field", login.Chaptcha }
            };
            if (login.ChaptchaSrc == string.Empty)
            {
                parameters["captcha_challenge_field"] = string.Empty;
            }
            var encoded_parameters = new FormUrlEncodedContent(parameters);
            try
            {
                HttpResponseMessage response = await HTMLclient.PostAsync("login", encoded_parameters);
                response.EnsureSuccessStatusCode();
                CookieCollection response_cookie = Cookies.GetCookies(HTMLclient.BaseAddress);
                Settings.Default.Cookies = response_cookie;

                IEnumerable<Cookie> response_cookies = Cookies.GetCookies(HTMLclient.BaseAddress).Cast<Cookie>();
                foreach (Cookie cookie in response_cookies)
                {
                    Console.WriteLine(cookie.Name + ": " + cookie.Value);
                }

                login_result.Status = 1;
                return login_result;
            }
            catch (HttpRequestException e)
            {
                login_result.Status = -1;
                login_result.ErrorMessage = e.Message;
                return login_result;
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
            string response_body;
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
                HttpResponseMessage response = await JSONclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                response_body = await response.Content.ReadAsStringAsync();
                subject_result.Status = 1;
            }
            catch (HttpRequestException e)
            {
                subject_result.Status = -1;
                subject_result.ErrorMessage = e.Message;
                return subject_result;
            }
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

    }

}
