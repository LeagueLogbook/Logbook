using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class FinishPasswordResetCommandHandler : ICommandHandler<FinishPasswordResetCommand, object>
    {
        private readonly ISession _session;
        private readonly ISecretGenerator _secretGenerator;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueue _emailQueue;
        private readonly IHashingService _hashingService;

        public FinishPasswordResetCommandHandler(ISession session, ISecretGenerator secretGenerator, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailQueue emailQueue, IHashingService hashingService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(secretGenerator, nameof(secretGenerator));
            Guard.NotNull(encryptionService, nameof(encryptionService));
            Guard.NotNull(emailTemplateService, nameof(emailTemplateService));
            Guard.NotNull(emailQueue, nameof(emailQueue));
            Guard.NotNull(hashingService, nameof(hashingService));

            this._session = session;
            this._secretGenerator = secretGenerator;
            this._encryptionService = encryptionService;
            this._emailTemplateService = emailTemplateService;
            this._emailQueue = emailQueue;
            this._hashingService = hashingService;
        }

        public async Task<object> Execute(FinishPasswordResetCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var emailAddress = this._encryptionService.ValidateAndDecodeForPasswordReset(command.Token);

            var user = this._session.Query<User>()
                .Where(f => f.EmailAddress.ToUpper() == emailAddress.Trim().ToUpper())
                .FetchMany(f => f.Authentications)
                .AsEnumerable() //I need this call here because FirstOrDefault will use SQL paging which doesnt correctly work with FetchMany
                .FirstOrDefault();

            if (user == null)
                throw new UserNotFoundException();

            var newPassword = this._secretGenerator.GenerateString(Config.Security.PasswordResetNewPasswordLength);
            var newPasswordSHA256Hash = this._hashingService.ComputeSHA256Hash(newPassword);

            await scope.Execute(new ChangePasswordCommand(user, newPasswordSHA256Hash));

            var emailTemplate = new PasswordResettedEmailTemplate
            {
                NewPassword = newPassword
            };
            var email = this._emailTemplateService.GetTemplate(emailTemplate);
            email.Receiver = emailAddress;

            await this._emailQueue.EnqueueMailAsync(email);

            return new object();
        }
    }
}