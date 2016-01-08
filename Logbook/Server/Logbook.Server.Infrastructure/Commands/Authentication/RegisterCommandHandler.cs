using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Logbook.Localization.Server;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Authentication;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Extensions;
using Raven.Client;
using Raven.Client.Linq;

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, object>
    {
        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSender _emailSender;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="emailTemplateService">The email template service.</param>
        /// <param name="emailSender">The email sender.</param>
        public RegisterCommandHandler([NotNull]IAsyncDocumentSession documentSession, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailSender emailSender)
        {
            Guard.AgainstNullArgument(nameof(documentSession), documentSession);
            Guard.AgainstNullArgument(nameof(encryptionService), encryptionService);
            Guard.AgainstNullArgument(nameof(emailTemplateService), emailTemplateService);
            Guard.AgainstNullArgument(nameof(emailSender), emailSender);

            this._documentSession = documentSession;
            this._encryptionService = encryptionService;
            this._emailTemplateService = emailTemplateService;
            this._emailSender = emailSender;
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
            var emailAddressAlreadyInUse = await this._documentSession
                .Query<User, Users_ByEmailAddress>()
                .Where(f => f.EmailAddress == command.EmailAddress)
                .AnyAsync()
                .WithCurrentCulture();

            if (emailAddressAlreadyInUse)
                throw new EmailIsNotAvailableException();

            var token = this._encryptionService.GenerateForConfirmEmail(command.EmailAddress, command.PreferredLanguage, command.PasswordSHA256Hash);

            var emailTemplate = new ConfirmEmailEmailTemplate
            {
                Url = $"{command.OwinContext.Request.Scheme}://{command.OwinContext.Request.Host}/Authentication/Register/Finish?token={token}",
            };

            var email = this._emailTemplateService.GetTemplate(emailTemplate);
            email.Receiver = command.EmailAddress;

            await this._emailSender
                .SendMailAsync(email)
                .WithCurrentCulture();

            return new object();
        }
        #endregion
    }
}