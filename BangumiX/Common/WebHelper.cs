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

    public static class Retry
    {
        public static void Do(
            Action action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        public static T Do<T>(
            Func<T> action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
