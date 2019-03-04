using BangumiX.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Common
{
    class ApiHelper : WebHelper
    {
        public static async Task<List<Model.Collection>> GetWatching(uint id, string cat = "watching")
        {
            string url = string.Format("user/{0}/collection?app_id={1}&cat={2}", id, Settings.Default.ClientID, cat);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    List<Model.Collection> watching = JsonConvert.DeserializeObject<List<Model.Collection>>(response_body);
                    return watching;
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

        public static async Task<List<Model.MyCollection>> GetMyCollection(uint id, string subject_type = "anime")
        {
            string url = string.Format("user/{0}/collections/{1}?app_id={2}", id, subject_type, Settings.Default.ClientID);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    List<Model.MyCollectionWrapper> collection_list = JsonConvert.DeserializeObject<List<Model.MyCollectionWrapper>>(response_body);
                    return collection_list[0].collects;
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

        public static async Task<List<Model.DailyCollection>> GetDaily()
        {
            string url = string.Format("calendar");
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    List<Model.DailyCollection> daily_collections = JsonConvert.DeserializeObject<List<Model.DailyCollection>>(response_body);
                    return daily_collections;
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

        public static async Task<Model.SearchCollection> GetSearch(string keyword, uint start = 0)
        {
            string url = string.Format("search/subject/{0}?max_results=25&start={1}&type=2", keyword, start);
            using (HttpResponseMessage response = await APIclient.GetAsync(Uri.EscapeUriString(url)))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Model.SearchCollection search_collection = JsonConvert.DeserializeObject<Model.SearchCollection>(response_body);
                    return search_collection;
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
                catch (JsonReaderException json_reader_exception)
                {
                    throw new EmptySearchException(json_reader_exception.Message);
                }
            }
        }


        public static async Task<Model.SubjectLarge> GetSubject(uint id)
        {
            string url = string.Format("subject/{0}?responseGroup=large", id);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Model.SubjectLarge subject = JsonConvert.DeserializeObject<Model.SubjectLarge>(response_body);
                    return subject;
                }
                catch (HttpRequestException http_exception)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new WebException(http_exception.Message);

                    }
                    else
                    {
                        throw;
                    }
                }
                catch (WebException)
                {
                    throw;
                }
            }
        }

        public static async Task<Model.SubjectProgress> GetProgress(uint u_id, uint s_id)
        {
            string url = string.Format("user/{0}/progress?subject_id={1}", u_id, s_id);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Model.SubjectProgress subject_progress = JsonConvert.DeserializeObject<Model.SubjectProgress>(response_body);
                    return subject_progress;
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

        public static async Task<Model.User> GetUser(uint id)
        {
            string url = string.Format("user/{0}", id);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string response_body = await response.Content.ReadAsStringAsync();
                    Model.User user = JsonConvert.DeserializeObject<Model.User>(response_body);
                    return user;
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

        public static async Task UpdateCollection(uint id, string status)
        {
            Dictionary<string, string> Data = new Dictionary<string, string>()
                {
                    { "status", status }
                };
            FormUrlEncodedContent content = new FormUrlEncodedContent(Data);
            string url = string.Format("/collection/{0}/update", id);
            using (HttpResponseMessage response = await APIclient.PostAsync(url, content))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    return;
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
