using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Logbook.Worker.Api;
using Logbook.Worker.EmailSender;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Logbook.Azure.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private CancellationTokenSource _cancellationTokenSource;

        private ApiWorker _apiWoker;
        private EmailSenderWorker _emailSenderWorker;

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 100;

            var container = new WindsorContainer();
            container.Install(FromAssembly.InThisApplication());

            this._cancellationTokenSource = new CancellationTokenSource();

            this._apiWoker = container.Resolve<ApiWorker>();
            this._emailSenderWorker = container.Resolve<EmailSenderWorker>();

            Task.WaitAll(
                this._apiWoker.StartAsync(),
                this._emailSenderWorker.StartAsync());

            return true;
        }

        public override void Run()
        {
            Task.WaitAll(
                this._apiWoker.RunAsync(this._cancellationTokenSource.Token),
                this._emailSenderWorker.RunAsync(this._cancellationTokenSource.Token));
        }

        public override void OnStop()
        {
            this._cancellationTokenSource.Cancel();
        }
    }
}
