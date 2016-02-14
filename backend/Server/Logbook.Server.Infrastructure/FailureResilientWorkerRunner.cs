using System;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Server.Contracts;

namespace Logbook.Server.Infrastructure
{
    public static class FailureResilientWorkerRunner
    {
        public static async Task RunAsync(IWorker worker, CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await worker.RunAsync(cancellationToken);
                }
                catch (Exception exception) when (exception is TaskCanceledException == false)
                {
                    //TODO: Log error
                }
            }
        }
    }
}