using HashServer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyToHashServer
{
	static class Program
	{
		private static readonly Random rand = new Random();
		private const int defaultPort = 20000;
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));


		private static readonly string[] hashServers = //пока это будет тут, потом перенесу в файл, если нужно будет.
			new string[]
			{
				"127.0.0.1:21612",
				"127.0.0.1:21613",
				"127.0.0.1:21614",
				"127.0.0.1:21615",
				"127.0.0.1:21616",
			};

		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			try
			{
				var listener = new Listener(defaultPort, "method", OnContextAsync);
				listener.Start();

				log.InfoFormat("Proxy server started on port {0}", defaultPort);
				new ManualResetEvent(false).WaitOne();
			}
			catch (Exception e)
			{
				log.Fatal(e);
				throw;
			}

			//var ms = DownloadWebPageAsync("127.0.0.1:19999", "1").Result;
			//Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
		}

		private static async Task OnContextAsync(HttpListenerContext context)
		{
			var requestId = Guid.NewGuid();
			var query = context.Request.QueryString["query"];
			var remoteEndPoint = context.Request.RemoteEndPoint;
			log.InfoFormat("{0}: received {1} from {2}", requestId, query, remoteEndPoint);
			context.Request.InputStream.Close();
			MemoryStream ms = null;
			while (ms == null)
			{
				try
				{
					ms = await DownloadWebPageAsync(hashServers[rand.Next(hashServers.Length)], query);
				}
				catch (Exception)
				{
					log.InfoFormat("Server down");
				}
			}
			var encryptedBytes = ms.ToArray(); 

			await context.Response.OutputStream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);
			context.Response.OutputStream.Close();
			log.InfoFormat("{0}: {1} sent back to {2}", requestId, Encoding.UTF8.GetString(encryptedBytes), remoteEndPoint);
		}

		public static async Task<MemoryStream> DownloadWebPageAsync(string ipAndPortOfServer, string query)
		{
			var sw = Stopwatch.StartNew();
			var request = CreateRequest(String.Format("http://{0}/method?query={1}", ipAndPortOfServer, query));
			var response = await request.GetResponseAsync();
			var ms = new MemoryStream();
			using (var stream = response.GetResponseStream())
			{
				await stream.CopyToAsync(ms);
				Console.WriteLine("Got {0} bytes in {1} ms", ms.Position, sw.ElapsedMilliseconds);
			}
			return ms;
		}

		private static HttpWebRequest CreateRequest(string uriStr, int timeout = 5)
		{
			var request = WebRequest.CreateHttp(uriStr);
			request.Timeout = timeout;
			request.Proxy = null;
			request.KeepAlive = true;
			request.ServicePoint.UseNagleAlgorithm = false;
			request.ServicePoint.ConnectionLimit = 100;
			return request;
		}
	}
}
