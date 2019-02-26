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
    }
}
