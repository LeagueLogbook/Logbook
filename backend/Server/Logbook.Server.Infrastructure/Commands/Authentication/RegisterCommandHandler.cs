using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Localization.Server;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, object>
    {
        #region Fields
        private readonly ISession _session;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueue _emailQueue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="session">The database session.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="emailTemplateService">The email template service.</param>
        /// <param name="emailQueue">The email queue.</param>
        public RegisterCommandHandler(ISession session, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailQueue emailQueue)
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
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified <paramref name="command" />.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public async Task<object> Execute(RegisterCommand command, ICommandScope scope)
        {
            var emailAddressAlreadyInUse = this._session.Query<User>()
                .Any(f => f.EmailAddress.ToUpper() == command.EmailAddress.Trim().ToUpper());

            if (emailAddressAlreadyInUse)
                throw new EmailIsNotAvailableException();

            var token = this._encryptionService.GenerateForConfirmEmail(command.EmailAddress, command.PreferredLanguage, command.PasswordSHA256Hash);

            var emailTemplate = new ConfirmEmailEmailTemplate
            {
                Url = $"{command.OwinContext.Request.Scheme}://{command.OwinContext.Request.Host}/Authentication/Register/Finish?token={token}",
            };

            var email = this._emailTemplateService.GetTemplate(emailTemplate);
            email.Receiver = command.EmailAddress;

            await this._emailQueue.EnqueueMailAsync(email);

            return new object();
        }
        #endregion
    }
}