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
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, object>
    {
        private readonly ISession _session;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueue _emailQueue;

        public ResetPasswordCommandHandler(ISession session, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailQueue emailQueue)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(encryptionService, nameof(encryptionService));
            Guard.NotNull(emailTemplateService, nameof(emailTemplateService));
            Guard.NotNull(emailQueue, nameof(emailQueue));

            this._session = session;
            this._encryptionService = encryptionService;
            this._emailTemplateService = emailTemplateService;
            this._emailQueue = emailQueue;
        }

        public async Task<object> Execute(ResetPasswordCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var user = this._session.Query<User>()
                .Where(f => f.EmailAddress.ToUpper() == command.EmailAddress.Trim().ToUpper())
                .FetchMany(f => f.Authentications)
                .AsEnumerable() //I need this call here because FirstOrDefault will use SQL paging which doesnt correctly work with FetchMany
                .FirstOrDefault();
            
            if (user == null)
                throw new UserNotFoundException();

            if (user.Authentications.Any(f => f is LogbookAuthenticationKind) == false)
                throw new NoLogbookLoginToResetPasswordException();

            var token = this._encryptionService.GenerateForPasswordReset(command.EmailAddress);

            var emailTemplate = new ResetPasswordEmailTemplate
            {
                Url = $"{command.OwinContext.Request.Scheme}://{command.OwinContext.Request.Host}/Authentication/PasswordReset/Finish?token={token}",
            };

            var email = this._emailTemplateService.GetTemplate(emailTemplate);
            email.Receiver = user.EmailAddress;

            await this._emailQueue.EnqueueMailAsync(email);

            return new object();
        }
    }
}