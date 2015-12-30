using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, object>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IJsonWebTokenService _jsonWebTokenService;

        public ResetPasswordCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService)
        {
            this._documentSession = documentSession;
            this._jsonWebTokenService = jsonWebTokenService;
        }

        public async Task<object> Execute(ResetPasswordCommand command, ICommandScope scope)
        {
            var user = await this._documentSession
                .Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == command.EmailAddress)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            if (authenticationData.Authentications.Any(f => f.Kind == AuthenticationKind.Logbook) == false)
            {
                var authenticationKinds = authenticationData.Authentications
                    .Select(f => f.Kind)
                    .ToList();

                throw new LogbookException("Dummy Text");
            }

            var token = this._jsonWebTokenService.GenerateForPasswordReset(command.EmailAddress);

            //TODO: Generate email template
            //TODO: Send email

            return new object();
        }
    }
}