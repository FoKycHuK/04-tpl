﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace ProxyToHashServer
{
    public class Listener
    {
        private HttpListener listener;

        public Listener(int port, string suffix, Func<HttpListenerContext, Task> callbackAsync)
        {
            ThreadPool.SetMinThreads(8, 8);
            CallbackAsync = callbackAsync;
            listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://+:{0}{1}/", port, suffix != null ? "/" + suffix.TrimStart('/') : ""));
        }

        public void Start()
        {
            listener.Start();
            StartListen();
        }

        public async void StartListen()
        {
            while (true)
            {
                var context = await listener.GetContextAsync();

                Task.Run(
                    async () =>
                    {
                        var ctx = context;
                        try
                        {
                            await CallbackAsync(ctx);
                        }
                        finally
                        {
                            ctx.Response.Close();
                        }
                    }
                );
            }
        }

        private Func<HttpListenerContext, Task> CallbackAsync { get; set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
    }
}
