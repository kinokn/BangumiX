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
            var timePast = (int)(DateTimeOffset.Now - Settings.TokenTime).TotalSeconds;
            if (timePast < Settings.Expire / 2)
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Model.Token token = JsonConvert.DeserializeObject<Model.Token>(responseBody);
                    token.token_time = DateTime.Now;
                    APIclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Settings.TokenType, Settings.AccessToken);
                }
                catch (HttpRequestException httpException)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new WebException(httpException.Message);
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
            try
            {
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startURI, endURI);
                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string url = "callback" + webAuthenticationResult.ResponseData.ToString().Substring(endURL.Length);
                    using (HttpResponseMessage response = await TokenClient.GetAsync(url))
                    {
                        try
                        {
                            response.EnsureSuccessStatusCode();
                            string response_body = await response.Content.ReadAsStringAsync();
                            Model.Token token = JsonConvert.DeserializeObject<Model.Token>(response_body);
                            token.token_time = DateTimeOffset.Now;
                            APIclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Settings.TokenType, Settings.AccessToken);
                        }
                        catch (HttpRequestException httpException)
                        {
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                throw new WebException(httpException.Message);
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
                else
                {
                    throw new Exception(webAuthenticationResult.ResponseErrorDetail.ToString());
                }

            }
            catch
            {
                throw;
            }

        }

    }
}
