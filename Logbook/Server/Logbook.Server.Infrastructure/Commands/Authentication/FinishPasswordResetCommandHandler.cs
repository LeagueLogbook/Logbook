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
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FinishPasswordResetCommandHandler : ICommandHandler<FinishPasswordResetCommand, object>
    {
        private readonly IAsyncDocumentSession _documentSession;
        private readonly ISecretGenerator _secretGenerator;
        private readonly ISaltCombiner _saltCombiner;
        private readonly IJsonWebTokenService _jsonWebTokenService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSender _emailSender;

        public FinishPasswordResetCommandHandler(IAsyncDocumentSession documentSession, ISecretGenerator secretGenerator, ISaltCombiner saltCombiner, IJsonWebTokenService jsonWebTokenService, IEmailTemplateService emailTemplateService, IEmailSender emailSender)
        {
            this._documentSession = documentSession;
            this._secretGenerator = secretGenerator;
            this._saltCombiner = saltCombiner;
            this._jsonWebTokenService = jsonWebTokenService;
            this._emailTemplateService = emailTemplateService;
            this._emailSender = emailSender;
        }

        public async Task<object> Execute(FinishPasswordResetCommand command, ICommandScope scope)
        {
            var decryptedToken = this._jsonWebTokenService.ValidateAndDecodeForPasswordReset(command.Token);

            var user = await this._documentSession
                .Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == decryptedToken)
                .FirstOrDefaultAsync()
                .WithCurrentCulture();

            var authenticationData = await this._documentSession
                .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
                .WithCurrentCulture();

            var logbookAuthentication = authenticationData.Authentications
                .OfType<LogbookAuthenticationKind>()
                .First();

            var newPassword = Convert.ToBase64String(this._secretGenerator.Generate(16));
            var newPasswordSHA256Hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(newPassword));

            var salt = this._secretGenerator.Generate();

            logbookAuthentication.IterationCount = Config.IterationCountForPasswordHashing;
            logbookAuthentication.Salt = salt;
            logbookAuthentication.Hash = this._saltCombiner.Combine(salt, Config.IterationCountForPasswordHashing, newPasswordSHA256Hash);

            var emailTemplate = new PasswordResettedEmailTemplate
            {
                NewPassword = newPassword
            };
            var email = this._emailTemplateService.GetTemplate(emailTemplate);
            email.Receiver = user.EmailAddress;

            await this._emailSender
                .SendMailAsync(email)
                .WithCurrentCulture();

            return new object();
        }
    }
}