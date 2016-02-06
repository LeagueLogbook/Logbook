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

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FinishPasswordResetCommandHandler : ICommandHandler<FinishPasswordResetCommand, object>
    {
        //private readonly IAsyncDocumentSession _documentSession;
        //private readonly ISecretGenerator _secretGenerator;
        //private readonly IEncryptionService _encryptionService;
        //private readonly IEmailTemplateService _emailTemplateService;
        //private readonly IEmailSender _emailSender;
        //private readonly IHashingService _hashingService;

        //public FinishPasswordResetCommandHandler(IAsyncDocumentSession documentSession, ISecretGenerator secretGenerator, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailSender emailSender, IHashingService hashingService)
        //{
        //    this._documentSession = documentSession;
        //    this._secretGenerator = secretGenerator;
        //    this._encryptionService = encryptionService;
        //    this._emailTemplateService = emailTemplateService;
        //    this._emailSender = emailSender;
        //    this._hashingService = hashingService;
        //}

        public async Task<object> Execute(FinishPasswordResetCommand command, ICommandScope scope)
        {
            throw new NotImplementedException();
            //var emailAddress = this._encryptionService.ValidateAndDecodeForPasswordReset(command.Token);

            //var user = await this._documentSession
            //    .Query<User, Users_ByEmailAddress>()
            //    .Where(f => f.EmailAddress == emailAddress)
            //    .FirstAsync()
            //    .WithCurrentCulture();

            //var authenticationData = await this._documentSession
            //    .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
            //    .WithCurrentCulture();

            //var newPassword = this._secretGenerator.GenerateString(Config.PasswordResetNewPasswordLength);
            //var newPasswordSHA256Hash = this._hashingService.ComputeSHA256Hash(newPassword);

            //await scope
            //    .Execute(new ChangePasswordCommand(user, authenticationData, newPasswordSHA256Hash))
            //    .WithCurrentCulture();

            //var emailTemplate = new PasswordResettedEmailTemplate
            //{
            //    NewPassword = newPassword
            //};
            //var email = this._emailTemplateService.GetTemplate(emailTemplate);
            //email.Receiver = emailAddress;

            //await this._emailSender
            //    .SendMailAsync(email)
            //    .WithCurrentCulture();

            //return new object();
        }
    }
}