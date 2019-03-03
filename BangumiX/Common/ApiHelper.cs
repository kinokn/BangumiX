using BangumiX.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Common
{
    class ApiHelper : WebHelper
    {
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
            public List<Model.MyCollectionWrapper> CollectWrapper { get; set; }
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
                collection_result.CollectWrapper = JsonConvert.DeserializeObject<List<Model.MyCollectionWrapper>>(response_body);
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

        public class ProgressResult : HttpResult
        {
            public Model.SubjectProgress SubjectProgress { get; set; }
        }
        public static async Task<ProgressResult> GetProgress(uint u_id, uint s_id)
        {
            ProgressResult progress_result = new ProgressResult();
            try
            {
                string url = String.Format("user/{0}/progress?subject_id={1}", u_id, s_id);
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                progress_result.SubjectProgress = JsonConvert.DeserializeObject<Model.SubjectProgress>(response_body);
                progress_result.Status = 1;
                return progress_result;
            }
            catch (HttpRequestException e)
            {
                progress_result.Status = -1;
                progress_result.ErrorMessage = e.Message;
                return progress_result;
            }
        }

        public class DailyResult : HttpResult
        {
            public List<Model.DailyCollection> DailyCollections { get; set; }
        }
        public static async Task<DailyResult> GetDaily()
        {
            DailyResult daily_result = new DailyResult();
            try
            {
                string url = String.Format("calendar");
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                daily_result.DailyCollections = JsonConvert.DeserializeObject<List<Model.DailyCollection>>(response_body);
                daily_result.Status = 1;
                return daily_result;
            }
            catch (HttpRequestException e)
            {
                daily_result.Status = -1;
                daily_result.ErrorMessage = e.Message;
                return daily_result;
            }
        }

        public class SearchResult : HttpResult
        {
            public Model.SearchCollection SearchCollections { get; set; }
        }
        public static async Task<SearchResult> GetSearch(string keyword, uint start = 0)
        {
            SearchResult search_result = new SearchResult();
            try
            {
                string url = String.Format("search/subject/{0}?max_results=25&start={1}&type=2", keyword, start);
                HttpResponseMessage response = await APIclient.GetAsync(Uri.EscapeUriString(url));
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                search_result.SearchCollections = JsonConvert.DeserializeObject<Model.SearchCollection>(response_body);
                search_result.Status = 1;
                return search_result;
            }
            catch (HttpRequestException e)
            {
                search_result.Status = -1;
                search_result.ErrorMessage = e.Message;
                return search_result;
            }
            catch (Exception e)
            {
                search_result.Status = -1;
                search_result.ErrorMessage = e.Message;
                return search_result;
            }
        }

        public class UserResult : HttpResult
        {
            public Model.User User { get; set; }
        }
        public static async Task<UserResult> GetUser(uint id)
        {
            UserResult user_result = new UserResult();
            try
            {
                string url = String.Format("user/{0}", id);
                HttpResponseMessage response = await APIclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string response_body = await response.Content.ReadAsStringAsync();
                user_result.User = JsonConvert.DeserializeObject<Model.User>(response_body);
                user_result.Status = 1;
                return user_result;
            }
            catch (HttpRequestException e)
            {
                user_result.Status = -1;
                user_result.ErrorMessage = e.Message;
                return user_result;
            }
        }

        public static async Task<HttpResult> UpdateCollection(uint id, string type)
        {
            HttpResult update_collection_result = new HttpResult();
            try
            {
                Dictionary<string, string> Data = new Dictionary<string, string>()
                {
                    { "status", type }
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(Data);
                string url = String.Format("/collection/{0}/update", id);
                HttpResponseMessage response = await APIclient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                update_collection_result.Status = 1;
                return update_collection_result;
            }
            catch (HttpRequestException e)
            {
                update_collection_result.Status = -1;
                update_collection_result.ErrorMessage = e.Message;
                return update_collection_result;
            }
        }
    }
}
