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

        public static async Task<dynamic> GetSubject(int id, int responseGroup = 0)
        {

            string responseBody = await GetSubjectString(id, responseGroup);
            dynamic subject = new SubjectSmall();
            switch (responseGroup)
            {
                case 0:
                    subject = JsonConvert.DeserializeObject<SubjectSmall>(responseBody);
                    break;
                case 1:
                    subject = JsonConvert.DeserializeObject<SubjectLarge>(responseBody);
                    break;
                case 2:
                    subject = JsonConvert.DeserializeObject<SubjectLarge>(responseBody);
                    break;
            }
            return subject;
        }

        public static async Task<string> GetSubjectString(int id, int responseGroup = 0)
        {
            try
            {
                string url = String.Format("subject/{0}?responseGroup=", id);
                switch (responseGroup)
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

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
        }
    }
}
