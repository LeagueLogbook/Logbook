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
    public class FinishRegistrationCommandHandler : ICommandHandler<FinishRegistrationCommand, User>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IJsonWebTokenService _jsonWebTokenService;

        public FinishRegistrationCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService)
        {
            this._documentSession = documentSession;
            this._jsonWebTokenService = jsonWebTokenService;
        }

        public async Task<User> Execute(FinishRegistrationCommand command, ICommandScope scope)
        {
            var decryptedToken = this._jsonWebTokenService.ValidateAndDecodeForConfirmEmail(command.Token);

            var emailAddressAlreadyInUse = await this._documentSession
                .Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == decryptedToken.EmailAddress)
                .AnyAsync()
                .WithCurrentCulture();

            if (emailAddressAlreadyInUse)
                throw new EmailIsNotAvailableException();

            var user = new User
            {
                EmailAddress = decryptedToken.EmailAddress,
                PreferredLanguage = decryptedToken.PreferredLanguage
            };

            await this._documentSession
                .StoreAsync(user)
                .WithCurrentCulture();

            var authenticationData = new AuthenticationData
            {
                ForUserId = user.Id,
            };

            await scope
                .Execute(new ChangePasswordCommand(user, authenticationData, decryptedToken.PasswordSHA256Hash))
                .WithCurrentCulture();

            await this._documentSession
                .StoreAsync(authenticationData)
                .WithCurrentCulture();

            return user;
        }
    }
}