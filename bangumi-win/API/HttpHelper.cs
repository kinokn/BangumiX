using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Net.Http;
using Newtonsoft.Json;
using HtmlAgilityPack;


namespace bangumi_win.API
{
    class HttpHelper
    {
        static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/")
        };

        public static async Task<dynamic> CheckLogin()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseBody);
                var form = htmlDoc.DocumentNode.SelectSingleNode("//form[@id=\"loginForm\"]");
                if (form != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
        }

        public class SubjectResult
        {
            public int Status { get; set; }
            public string ErrorMessage { get; set; }
            public dynamic Subject { get; set; }
        }
        public static async Task<SubjectResult> GetSubject(int id, int response_group = 0)
        {
            string response_body;
            SubjectResult subject_result = new SubjectResult();
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
                HttpResponseMessage _response = await client.GetAsync(url);
                _response.EnsureSuccessStatusCode();
                response_body = await _response.Content.ReadAsStringAsync();
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
