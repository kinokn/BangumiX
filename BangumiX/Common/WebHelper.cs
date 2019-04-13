using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BangumiX.Common
{
    public abstract class WebHelper
    {
        public static readonly HttpClient APIclient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.bgm.tv/"),
            Timeout = new TimeSpan(0, 0, 30)
        };
        public static readonly HttpClient TokenClient = new HttpClient()
        {
            BaseAddress = new Uri("http://47.101.195.180:5000/")
        };

        public class AuthorizationException : Exception
        {
            public AuthorizationException() { }
            public AuthorizationException(string message) : base(message) { }
            public AuthorizationException(string message, Exception inner) : base(message, inner) { }
        }
        public class EmptySearchException : Exception
        {
            public EmptySearchException() { }
            public EmptySearchException(string message) : base(message) { }
            public EmptySearchException(string message, Exception inner) : base(message, inner) { }
        }
    }
}
