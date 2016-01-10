using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Logbook.Server.Hosts.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HostFactory.Run(f =>
                {
                    f.Service<LogbookService>(d =>
                    {
                        d.ConstructUsing(_ => new LogbookService());
                        d.WhenStarted(x => x.Start());
                        d.WhenStopped(x => x.Stop());
                    });

                    f.RunAsLocalSystem();
                    f.StartAutomatically();

                    f.SetDescription("The Logbook HTTP Api.");
                    f.SetDisplayName("Logbook HTTP Api");
                    f.SetServiceName("LogbookHTTPApi");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
