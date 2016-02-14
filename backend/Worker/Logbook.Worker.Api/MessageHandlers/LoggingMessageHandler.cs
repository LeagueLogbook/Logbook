using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Shared;

namespace Logbook.Worker.Api.MessageHandlers
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        #region Overrides of DelegatingHandler
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Guard.NotNull(request, nameof(request));

            HttpResponseMessage result = await base.SendAsync(request, cancellationToken);
            
            return result;
        }
        #endregion
    }
}