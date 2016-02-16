using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Server.Infrastructure;
using Logbook.Server.Infrastructure.Exceptions;
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

            AppInsights.GenerateAsyncAwareOperationId();

            var startTime = DateTimeOffset.UtcNow;
            var watch = Stopwatch.StartNew();
            bool success = true;

            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception exception) when (exception is LogbookException == false)
            {
                success = false;
                throw;
            }
            finally
            {
                watch.Stop();

                var telemetry = new RequestTelemetry
                {
                    HttpMethod = request.Method.Method,
                    Duration = watch.Elapsed,
                    Success = success,
                    StartTime = startTime,
                    Url = request.RequestUri
                };

                AppInsights.Client.TrackRequest(telemetry);
            }
        }
        #endregion
    }
}