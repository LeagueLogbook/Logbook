using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Server.Infrastructure;
using Logbook.Shared;
using Microsoft.ApplicationInsights.DataContracts;

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

            var startTime = DateTimeOffset.UtcNow;
            var watch = Stopwatch.StartNew();
            HttpResponseMessage result = null;

            try
            {

                return result = await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                watch.Stop();

                var telemetry = new RequestTelemetry
                {
                    HttpMethod = request.Method.Method,
                    Duration = watch.Elapsed,
                    ResponseCode = result != null ? Enum.GetName(typeof(HttpStatusCode), result.StatusCode) : "Error",
                    Success = result != null,
                    StartTime = startTime,
                    Timestamp = DateTimeOffset.UtcNow,
                    Url = request.RequestUri
                };

                AppInsights.Client.TrackRequest(telemetry);
            }
        }
        #endregion
    }
}