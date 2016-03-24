using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FinishRegistrationCommandHandler : ICommandHandler<FinishRegistrationCommand, object>
    {
        private readonly ISession _session;
        private readonly IEncryptionService _encryptionService;

        public FinishRegistrationCommandHandler(ISession session, IEncryptionService encryptionService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(encryptionService, nameof(encryptionService));

            this._session = session;
            this._encryptionService = encryptionService;
        }

        public async Task<object> Execute(FinishRegistrationCommand command, ICommandScope scope)
        {
            Guard.NotNull(scope, nameof(scope));
            Guard.NotNull(command, nameof(command));

            var decryptedToken = this._encryptionService.ValidateAndDecodeForConfirmEmail(command.Token);

            var emailAddressAlreadyInUse = this._session.Query<User>()
                .Any(f => f.EmailAddress.ToUpper() == decryptedToken.EmailAddress.Trim().ToUpper());

            if (emailAddressAlreadyInUse)
                throw new EmailIsNotAvailableException();

            var user = new User
            {
                EmailAddress = decryptedToken.EmailAddress.Trim(),
                PreferredLanguage = decryptedToken.PreferredLanguage
            };

            this._session.SaveOrUpdate(user);
            
            await scope.Execute(new ChangePasswordCommand(user, decryptedToken.PasswordSHA256Hash));
            
            return new object();
        }
    }
}