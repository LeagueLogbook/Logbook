using System;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FinishRegistrationCommandHandler : ICommandHandler<FinishRegistrationCommand, object>
    {
        private readonly ISession _session;
        private readonly IEncryptionService _encryptionService;

        public FinishRegistrationCommandHandler(ISession session, IEncryptionService encryptionService)
        {
            this._session = session;
            this._encryptionService = encryptionService;
        }

        public async Task<object> Execute(FinishRegistrationCommand command, ICommandScope scope)
        {
            var decryptedToken = this._encryptionService.ValidateAndDecodeForConfirmEmail(command.Token);

            var emailAddressAlreadyInUse = this._session.QueryOver<User>()
                .WhereRestrictionOn(f => f.EmailAddress).IsInsensitiveLike(decryptedToken.EmailAddress)
                .RowCount() > 0;

            if (emailAddressAlreadyInUse)
                throw new EmailIsNotAvailableException();

            var user = new User
            {
                EmailAddress = decryptedToken.EmailAddress,
                PreferredLanguage = decryptedToken.PreferredLanguage
            };

            this._session.SaveOrUpdate(user);
            
            await scope
                .Execute(new ChangePasswordCommand(user, decryptedToken.PasswordSHA256Hash))
                .WithCurrentCulture();
            
            return new object();
        }
    }
}