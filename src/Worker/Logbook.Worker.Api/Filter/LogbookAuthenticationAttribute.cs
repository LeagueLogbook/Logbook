﻿using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Extensions;
using Logbook.Worker.Api.Extensions;

namespace Logbook.Worker.Api.Filter
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
            Guard.NotNull(context, nameof(context));

            var commandExecutor = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService<ICommandExecutor>();

            var userId = await commandExecutor
                .Execute(new AuthenticateCommand(context.Request.GetOwinContext()));

            if (userId > 0)
            {
                context.Principal = new GenericPrincipal(new GenericIdentity(userId.ToString()), new string[0]);
            }
            else
            {
                throw new InternalServerErrorException();
            }
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