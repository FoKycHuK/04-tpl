using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProxyToHashServer
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        //необходимые методы. пока оставлю тут.
        public static async Task DownloadWebPageAsync()
        {
            var sw = Stopwatch.StartNew();
            var request = CreateRequest("http://e1.ru");
            var response = await request.GetResponseAsync();
            using (var stream = response.GetResponseStream())
            {
                var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                Console.WriteLine("Got {0} bytes in {1} ms", ms.Position, sw.ElapsedMilliseconds);
            }
        }

        private static HttpWebRequest CreateRequest(string uriStr, int timeout = 30 * 1000)
        {
            var request = WebRequest.CreateHttp(uriStr);
            request.Timeout = timeout;
            request.Proxy = null;
            request.KeepAlive = true;
            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.ConnectionLimit = 4;
            return request;
        }
    }
}
