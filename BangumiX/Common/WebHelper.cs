using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Common
{
    public abstract class WebHelper
    {
        public static readonly HttpClient APIclient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/"),
            Timeout = new TimeSpan(0, 0, 10)
        };
        public static readonly HttpClient TokenClient = new HttpClient()
        {
            BaseAddress = new Uri("http://47.101.195.180:5000/")
        };
        public static readonly HttpClient HTMLclient = new HttpClient()
        {
            BaseAddress = new Uri("https://bangumi.tv/")
        };
        public static string host = HTMLclient.BaseAddress.ToString();

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

    }
}
