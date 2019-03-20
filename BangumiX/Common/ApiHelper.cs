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
            string url = string.Format("user/{0}/collection?app_id={1}&cat={2}", id, Settings.ClientID, cat);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Model.Collection> watching = JsonConvert.DeserializeObject<List<Model.Collection>>(responseBody);
                    return watching;
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

        public static async Task<List<Model.MyCollection>> GetMyCollection(uint id, string subject_type = "anime")
        {
            string url = string.Format("user/{0}/collections/{1}?app_id={2}", id, subject_type, Settings.ClientID);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Model.MyCollectionWrapper> collectionList = JsonConvert.DeserializeObject<List<Model.MyCollectionWrapper>>(responseBody);
                    return collectionList[0].collects;
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

        public static async Task<List<Model.DailyCollection>> GetDaily()
        {
            string url = string.Format("calendar");
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    List<Model.DailyCollection> dailyCollections = JsonConvert.DeserializeObject<List<Model.DailyCollection>>(responseBody);
                    return dailyCollections;
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

        public static async Task<Model.SearchCollection> GetSearch(string keyword, uint start = 0)
        {
            string url = string.Format("search/subject/{0}?max_results=25&start={1}&type=2", keyword, start);
            using (HttpResponseMessage response = await APIclient.GetAsync(Uri.EscapeUriString(url)))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Model.SearchCollection searchCollection = JsonConvert.DeserializeObject<Model.SearchCollection>(responseBody);
                    return searchCollection;
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
                catch (JsonReaderException jsonReaderException)
                {
                    throw new EmptySearchException(jsonReaderException.Message);
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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Model.SubjectLarge subject = JsonConvert.DeserializeObject<Model.SubjectLarge>(responseBody);
                    return subject;
                }
                catch (HttpRequestException httpException)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new WebException(httpException.Message);

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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Model.SubjectProgress subjectProgress = JsonConvert.DeserializeObject<Model.SubjectProgress>(responseBody);
                    return subjectProgress;
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

        public static async Task<Model.User> GetUser(uint id)
        {
            string url = string.Format("user/{0}", id);
            using (HttpResponseMessage response = await APIclient.GetAsync(url))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Model.User user = JsonConvert.DeserializeObject<Model.User>(responseBody);
                    return user;
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
    }
}
