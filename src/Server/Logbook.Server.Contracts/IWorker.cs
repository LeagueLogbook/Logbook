using System.Threading;
using System.Threading.Tasks;

namespace Logbook.Server.Contracts
{
    public interface IWorker
    {
        /// <summary>
        /// Startup code for the worker.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// The action the worker is actually doing.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task RunAsync(CancellationToken cancellationToken);
    }
}