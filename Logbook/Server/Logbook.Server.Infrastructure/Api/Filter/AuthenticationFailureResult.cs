using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using LiteGuard;
using Logbook.Server.Infrastructure.Extensions;

namespace Logbook.Server.Infrastructure.Api.Filter
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        #region Fields
        private readonly string _message;
        private readonly HttpRequestMessage _request;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFailureResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="request">The request.</param>
        public AuthenticationFailureResult(string message, HttpRequestMessage request)
        {
            Guard.AgainstNullArgument("message", message);
            Guard.AgainstNullArgument("request", request);

            this._message = message;
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
            var response = this._request.GetMessageWithError(HttpStatusCode.Unauthorized, this._message);

            return Task.FromResult(response);
        }
        #endregion
    }
}