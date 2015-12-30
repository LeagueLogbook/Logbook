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
        private readonly ISecretGenerator _secretGenerator;
        private readonly ISaltCombiner _saltCombiner;

        public FinishRegistrationCommandHandler(IAsyncDocumentSession documentSession, IJsonWebTokenService jsonWebTokenService, ISecretGenerator secretGenerator, ISaltCombiner saltCombiner)
        {
            this._documentSession = documentSession;
            this._jsonWebTokenService = jsonWebTokenService;
            this._secretGenerator = secretGenerator;
            this._saltCombiner = saltCombiner;
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

            await this._documentSession.StoreAsync(user).WithCurrentCulture();

            var salt = this._secretGenerator.Generate();
            var authenticationData = new AuthenticationData
            {
                ForUserId = user.Id,
                Authentications =
                {
                    new LogbookAuthenticationKind
                    {
                        Salt = salt,
                        IterationCount = Config.IterationCountForPasswordHashing,
                        Hash = this._saltCombiner.Combine(salt, Config.IterationCountForPasswordHashing, decryptedToken.PasswordSHA256Hash)
                    }
                }
            };

            await this._documentSession.StoreAsync(authenticationData).WithCurrentCulture();

            return user;
        }
    }
}