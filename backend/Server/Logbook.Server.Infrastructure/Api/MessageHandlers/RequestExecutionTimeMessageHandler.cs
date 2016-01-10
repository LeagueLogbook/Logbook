using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Metrics;
using Timer = Metrics.Timer;

namespace Logbook.Server.Infrastructure.Api.MessageHandlers
{
    public class RequestExecutionTimeMessageHandler : DelegatingHandler
    {
        #region Fields
        private readonly Timer _timer = Metric.Timer("Request Execution", Unit.Requests);
        #endregion

        #region Overrides of DelegatingHandler
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (this._timer.NewContext())
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
        #endregion
    }
}