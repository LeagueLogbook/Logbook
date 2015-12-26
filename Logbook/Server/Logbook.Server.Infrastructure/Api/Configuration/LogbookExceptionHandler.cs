using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using LiteGuard;
using Logbook.Localization.Server;
using Logbook.Server.Infrastructure.Exceptions;
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
            context.Result = new ExceptionResult(context);
        }
        #endregion

        #region Internal
        private class ExceptionResult : IHttpActionResult
        {
            private readonly ExceptionHandlerContext _context;

            /// <summary>
            /// Initializes a new instance of the <see cref="UnexpectedExceptionResult"/> class.
            /// </summary>
            /// <param name="context">The exception handler context.</param>
            public ExceptionResult(ExceptionHandlerContext context)
            {
                Guard.AgainstNullArgument(nameof(context), context);

                this._context = context;
            }

            /// <summary>
            /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
            /// </summary>
            /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpStatusCode statusCode = this.GetStatusCodeForException(this._context.Exception);
                string message = this.GetMessageForException(this._context.Exception);

                var response = this._context.Request.GetMessageWithError(statusCode, message);

                return Task.FromResult(response);
            }

            private HttpStatusCode GetStatusCodeForException(Exception exception)
            {
                var logbookException = exception as LogbookException;

                if (logbookException != null)
                {
                    return LogbookExceptionToStatusCodeMapping.GetStatusCode(logbookException);
                }

                return HttpStatusCode.InternalServerError;
            }

            private string GetMessageForException(Exception exception)
            {
                var logbookException = exception as LogbookException;
                return logbookException?.Message ?? ServerMessages.InternalServerError;
            }
        }
        #endregion
    }
}