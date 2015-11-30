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
using Logbook.Shared.Results;
using Microsoft.Owin;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, string>
    {
        #region Constants
        public const string AuthenticationMechanism = "Bearer";
        #endregion

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
        /// Executes this command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<Result<string>> Execute(AuthenticateCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument(nameof(command), command);
            Guard.AgainstNullArgument(nameof(scope), scope);

            Result<string> tokenResult = this.GetToken(command.Context);

            if (tokenResult.IsError)
                return tokenResult;

            return this._jsonWebTokenService.ValidateAndDecode(tokenResult.Data);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Tries to extract the JWT from the request.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetToken(IOwinContext requestContext)
        {
            Result<string> tokenFromUri = this.GetTokenFromUri(requestContext);

            if (tokenFromUri.IsSuccess)
                return tokenFromUri;

            Result<string> tokenFromHeader = this.GetTokenFromHeader(requestContext);

            if (tokenFromHeader.IsSuccess)
                return tokenFromHeader;

            return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);
        }
        /// <summary>
        /// Tries to extract the JWT from the request query parameters.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetTokenFromUri(IOwinContext requestContext)
        {
            IList<string> values = requestContext.Request.Query.GetValues("token");

            if (values != null && values.Any())
                return Result.AsSuccess(values.First());

            return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);
        }
        /// <summary>
        /// Tries to extract the JWT from the request headers.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetTokenFromHeader(IOwinContext requestContext)
        {
            string authorizationHeader = requestContext.Request.Headers.Get("Authorization");

            if (authorizationHeader == null)
                return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);

            string[] parts = authorizationHeader.Split(' ');

            if (parts.Length != 2)
                return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);

            if (parts[0].Equals(AuthenticationMechanism, StringComparison.InvariantCultureIgnoreCase) == false)
                return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);

            return Result.AsSuccess(parts[1]);
        }
        #endregion
    }
}