using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Logbook.Server.Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Logbook.Azure.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private CancellationTokenSource _cancellationTokenSource;
        private IWorker[] _workers;

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 100;

            var container = new WindsorContainer();
            container.Install(FromAssembly.InDirectory(new AssemblyFilter(".")));

            this._cancellationTokenSource = new CancellationTokenSource();

            this._workers = container.ResolveAll<IWorker>();

            Task.WaitAll(this._workers
                .Select(f => f.StartAsync())
                .ToArray());

            return true;
        }

        public override void Run()
        {
            Task.WaitAll(this._workers
                .Select(f => f.RunAsync(this._cancellationTokenSource.Token))
                .ToArray());;
        }

        public override void OnStop()
        {
            this._cancellationTokenSource.Cancel();
        }
    }
}
