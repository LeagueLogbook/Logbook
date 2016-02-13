using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Logbook.Server.Infrastructure.Exceptions;

namespace Logbook.Worker.Api.Filter
{
    public class OnlyLocalAttribute : Attribute, IAuthenticationFilter
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
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.ActionContext.RequestContext.IsLocal == false)
            {
                throw new OnlyLocalException();
            }

            return Task.CompletedTask;
        }
        /// <summary>
        /// Returns a challenge to the client application.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}