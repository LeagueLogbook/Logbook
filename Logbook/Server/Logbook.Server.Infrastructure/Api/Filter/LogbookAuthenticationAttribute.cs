using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using LiteGuard;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Extensions;

namespace Logbook.Server.Infrastructure.Api.Filter
{
    public class LogbookAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether you can apply this attribute multiple times.
        /// </summary>
        public bool AllowMultiple => false;
        #endregion

        #region Methods
        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var commandExecutor = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService<ICommandExecutor>();

            var isAuthenticated = await commandExecutor
                .Execute(new AuthenticateCommand(context.Request.GetOwinContext()))
                .WithCurrentCulture();

            if (isAuthenticated.IsSuccess)
            {
                context.Principal = new GenericPrincipal(new GenericIdentity(isAuthenticated.Data), new string[0]);
            }
            else
            {
                context.ErrorResult = new AuthenticationFailureResult(isAuthenticated.Message, context.Request);
            }
        }
        /// <summary>
        /// Returns a challenge to the client application.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(new object());
        }
        #endregion

        #region Internal
        private class AuthenticationFailureResult : IHttpActionResult
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
        #endregion
    }
}