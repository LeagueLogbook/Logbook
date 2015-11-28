using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Metrics;

namespace Logbook.Server.Infrastructure.Api.MessageHandlers
{
    public class ConcurrentRequestCountMessageHandler : DelegatingHandler
    {
        #region Fields
        private readonly Counter _counter = Metric.Counter("Concurrent Requests", Unit.Requests);
        #endregion

        #region Overrides of DelegatingHandler
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this._counter.Increment();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            this._counter.Decrement();

            return response;
        }
        #endregion
    }
}