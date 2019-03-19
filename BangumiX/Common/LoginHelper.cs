using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace BangumiX.Common
{
    public class LoginHelper : WebHelper
    {
        public static async Task<bool> CheckLogin()
        {
            if (Settings.AccessToken == string.Empty) return false;
            var time_remain = (int)(Settings.TokenTime - DateTimeOffset.Now).TotalSeconds;
            if (time_remain > Settings.Expire / 2)
            {
                APIclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Settings.TokenType, Settings.AccessToken);
                return true;
            }
            else
            {
                try
                {
                    await RefreshToken();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static async Task RefreshToken()
        {
            string url = string.Format("callback?refresh_token={0}", Settings.RefreshToken);
            using (HttpResponseMessage response = await TokenClient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Model.Token token = JsonConvert.DeserializeObject<Model.Token>(response_body);
                    token.token_time = DateTime.Now;
                }
                catch (HttpRequestException http_exception)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new WebException(http_exception.Message);
                    }
                    else
                    {
                        throw new AuthorizationException(response.StatusCode.ToString());
                    }
                }
                catch (WebException)
                {
                    throw;
                }
            }
        }

        public static async Task Login()
        {
            string startURL = string.Format("https://bgm.tv/oauth/authorize?client_id={0}&response_type=code", Settings.ClientID);
            string endURL = "http://47.101.195.180:5000/callback";
            Uri startURI = new Uri(startURL);
            Uri endURI = new Uri(endURL);
            string result;
            try
            {
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startURI, endURI);

                switch (webAuthenticationResult.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                    case WebAuthenticationStatus.ErrorHttp:
                        result = webAuthenticationResult.ResponseErrorDetail.ToString();
                        break;
                    default:
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            string url = "callback" + result.Substring(endURL.Length);
            using (HttpResponseMessage response = await TokenClient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Model.Token token = JsonConvert.DeserializeObject<Model.Token>(response_body);
                    token.token_time = DateTimeOffset.Now;
                }
                catch (HttpRequestException http_exception)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new WebException(http_exception.Message);
                    }
                    else
                    {
                        throw new AuthorizationException(response.StatusCode.ToString());
                    }
                }
                catch (WebException)
                {
                    throw;
                }
            }
        }

    }
}
