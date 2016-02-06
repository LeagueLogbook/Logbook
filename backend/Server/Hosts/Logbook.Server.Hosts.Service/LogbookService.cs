using System;
using System.Linq;
using Logbook.Server.Infrastructure;
using Microsoft.Owin.Hosting;

namespace Logbook.Server.Hosts.Service
{
    public class LogbookService
    {
        private IDisposable _webApp;

        public void Start()
        {
            try
            {
                var options = new StartOptions();
                foreach (var url in Config.Addresses.GetValue())
                {
                    options.Urls.Add(url.ToString());
                }

                this._webApp = WebApp.Start<Startup>(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Stop()
        {
            this._webApp?.Dispose();
        }
    }
}