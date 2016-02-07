using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, object>
    {
        private readonly ISession _session;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSender _emailSender;

        public ResetPasswordCommandHandler(ISession session, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailSender emailSender)
        {
            this._session = session;
            this._encryptionService = encryptionService;
            this._emailTemplateService = emailTemplateService;
            this._emailSender = emailSender;
        }

        public async Task<object> Execute(ResetPasswordCommand command, ICommandScope scope)
        {
            var user = this._session.QueryOver<User>()
                .WhereRestrictionOn(f => f.EmailAddress).IsInsensitiveLike(command.EmailAddress)
                .List()
                .First();
            
            if (user.Authentications.Any(f => f is LogbookAuthenticationKind) == false)
                throw new NoLogbookLoginToResetPasswordException();

            var token = this._encryptionService.GenerateForPasswordReset(command.EmailAddress);

            var emailTemplate = new ResetPasswordEmailTemplate
            {
                Url = $"{command.OwinContext.Request.Scheme}://{command.OwinContext.Request.Host}/Authentication/PasswordReset/Finish?token={token}",
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