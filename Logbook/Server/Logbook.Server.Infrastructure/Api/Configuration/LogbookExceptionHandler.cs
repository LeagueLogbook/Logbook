using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using LiteGuard;
using Logbook.Localization.Server;
using Logbook.Server.Infrastructure.Extensions;

namespace Logbook.Server.Infrastructure.Api.Configuration
{
    public class LogbookExceptionHandler : ExceptionHandler
    {
        #region Methods
        /// <summary>
        /// When overridden in a derived class, handles the exception synchronously.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ExceptionResult(context.Request);
        }
        #endregion

        #region Internal
        private class ExceptionResult : IHttpActionResult
        {
            #region Fields
            private readonly HttpRequestMessage _request;
            #endregion

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="ExceptionResult"/> class.
            /// </summary>
            /// <param name="request">The request.</param>
            public ExceptionResult(HttpRequestMessage request)
            {
                Guard.AgainstNullArgument(nameof(request), request);

                this._request = request;
            }
            #endregion

            #region Methods
            /// <summary>
            /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
            /// </summary>
            /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = this._request.GetMessageWithError(HttpStatusCode.InternalServerError, ServerMessages.InternalServerError);

                return Task.FromResult(response);
            }
            #endregion
        }
        #endregion
    }
}