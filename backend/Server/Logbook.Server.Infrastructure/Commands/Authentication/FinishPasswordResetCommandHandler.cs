using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FinishPasswordResetCommandHandler : ICommandHandler<FinishPasswordResetCommand, object>
    {
        private readonly ISession _session;
        private readonly ISecretGenerator _secretGenerator;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSender _emailSender;
        private readonly IHashingService _hashingService;

        public FinishPasswordResetCommandHandler(ISession session, ISecretGenerator secretGenerator, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailSender emailSender, IHashingService hashingService)
        {
            this._session = session;
            this._secretGenerator = secretGenerator;
            this._encryptionService = encryptionService;
            this._emailTemplateService = emailTemplateService;
            this._emailSender = emailSender;
            this._hashingService = hashingService;
        }

        public async Task<object> Execute(FinishPasswordResetCommand command, ICommandScope scope)
        {
            var emailAddress = this._encryptionService.ValidateAndDecodeForPasswordReset(command.Token);

            var user = this._session.QueryOver<User>()
                .WhereRestrictionOn(f => f.EmailAddress).IsInsensitiveLike(emailAddress)
                .List()
                .First();

            var newPassword = this._secretGenerator.GenerateString(Config.PasswordResetNewPasswordLength);
            var newPasswordSHA256Hash = this._hashingService.ComputeSHA256Hash(newPassword);

            await scope
                .Execute(new ChangePasswordCommand(user, newPasswordSHA256Hash))
                .WithCurrentCulture();

            var emailTemplate = new PasswordResettedEmailTemplate
            {
                NewPassword = newPassword
            };
            var email = this._emailTemplateService.GetTemplate(emailTemplate);
            email.Receiver = emailAddress;

            await this._emailSender
                .SendMailAsync(email)
                .WithCurrentCulture();

            return new object();
        }
    }
}