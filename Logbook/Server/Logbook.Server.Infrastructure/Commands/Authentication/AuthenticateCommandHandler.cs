using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Localization.Server;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Microsoft.Owin;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, string>
    {
        #region Fields
        private readonly IJsonWebTokenService _jsonWebTokenService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateCommandHandler"/> class.
        /// </summary>
        /// <param name="jsonWebTokenService">The json web token service.</param>
        public AuthenticateCommandHandler([NotNull]IJsonWebTokenService jsonWebTokenService)
        {
            Guard.AgainstNullArgument(nameof(jsonWebTokenService), jsonWebTokenService);

            this._jsonWebTokenService = jsonWebTokenService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<string> Execute(AuthenticateCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            string token = this.GetToken(command.Context);

            if (token == null)
                throw new UnauthorizedException();

            return this._jsonWebTokenService.ValidateAndDecodeForLogin(token);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Tries to extract the JWT from the request.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private string GetToken(IOwinContext requestContext)
        {
            string tokenFromUri = this.GetTokenFromUri(requestContext);

            if (tokenFromUri != null)
                return tokenFromUri;

            string tokenFromHeader = this.GetTokenFromHeader(requestContext);

            if (tokenFromHeader != null)
                return tokenFromHeader;

            return null;
        }
        /// <summary>
        /// Tries to extract the JWT from the request query parameters.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private string GetTokenFromUri(IOwinContext requestContext)
        {
            IList<string> values = requestContext.Request.Query.GetValues(Constants.Authentication.AuthorizationQueryPart);
            return values?.FirstOrDefault();
        }
        /// <summary>
        /// Tries to extract the JWT from the request headers.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private string GetTokenFromHeader(IOwinContext requestContext)
        {
            string authorizationHeader = requestContext.Request.Headers.Get("Authorization");

            string[] parts = authorizationHeader?.Split(' ');

            if (parts?.Length != 2)
                return null;

            if (parts[0].Equals(Constants.Authentication.AuthorizationHeaderType, StringComparison.InvariantCultureIgnoreCase) == false)
                return null;

            return parts[1];
        }
        #endregion
    }
}