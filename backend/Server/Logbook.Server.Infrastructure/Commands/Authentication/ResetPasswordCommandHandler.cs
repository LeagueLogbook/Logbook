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

namespace Logbook.Server.Infrastructure.Commands.Authentication
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, object>
    {
        //private readonly IAsyncDocumentSession _documentSession;
        //private readonly IEncryptionService _encryptionService;
        //private readonly IEmailTemplateService _emailTemplateService;
        //private readonly IEmailSender _emailSender;

        //public ResetPasswordCommandHandler(IAsyncDocumentSession documentSession, IEncryptionService encryptionService, IEmailTemplateService emailTemplateService, IEmailSender emailSender)
        //{
        //    this._documentSession = documentSession;
        //    this._encryptionService = encryptionService;
        //    this._emailTemplateService = emailTemplateService;
        //    this._emailSender = emailSender;
        //}

        public async Task<object> Execute(ResetPasswordCommand command, ICommandScope scope)
        {
            throw new NotImplementedException();

            //var user = await this._documentSession
            //    .Query<User, Users_ByEmailAddress>()
            //    .Where(f => f.EmailAddress == command.EmailAddress)
            //    .FirstOrDefaultAsync()
            //    .WithCurrentCulture();

            //var authenticationData = await this._documentSession
            //    .LoadAsync<AuthenticationData>(AuthenticationData.CreateId(user.Id))
            //    .WithCurrentCulture();

            //if (authenticationData.Authentications.Any(f => f.Kind == AuthenticationKind.Logbook) == false)
            //    throw new NoLogbookLoginToResetPasswordException();

            //var token = this._encryptionService.GenerateForPasswordReset(command.EmailAddress);

            //var emailTemplate = new ResetPasswordEmailTemplate
            //{
            //    Url = $"{command.OwinContext.Request.Scheme}://{command.OwinContext.Request.Host}/Authentication/PasswordReset/Finish?token={token}",
            //};

            //var email = this._emailTemplateService.GetTemplate(emailTemplate);
            //email.Receiver = user.EmailAddress;

            //await this._emailSender
            //    .SendMailAsync(email)
            //    .WithCurrentCulture();

            //return new object();
        }
    }
}