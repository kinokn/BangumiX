using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;

namespace bangumi_win.API
{
    class HttpHelper
    {
        static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/")
        };

        public static async Task<string> GetSubject(int id, int responseGroup = 0)
        {
            try
            {
                string url = "subject/" + id;
                if (responseGroup != 0)
                {
                    url += "?responseGroup=";
                    if (responseGroup == 1)
                    {
                        url += "medium";
                    }
                    else
                    {
                        url += "large";
                    }
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
