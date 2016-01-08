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
    public class FinishRegistrationCommandHandler : ICommandHandler<FinishRegistrationCommand, object>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IEncryptionService _encryptionService;

        public FinishRegistrationCommandHandler(IAsyncDocumentSession documentSession, IEncryptionService encryptionService)
        {
            this._documentSession = documentSession;
            this._encryptionService = encryptionService;
        }

        public async Task<object> Execute(FinishRegistrationCommand command, ICommandScope scope)
        {
            var decryptedToken = this._encryptionService.ValidateAndDecodeForConfirmEmail(command.Token);

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

            return new object();
        }
    }
}