using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Microsoft.Owin;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class RegisterCommand : ICommand<object>
    {
        public RegisterCommand(string emailAddress, byte[] passwordSHA256Hash, string preferredLanguage, IOwinContext owinContext)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Guard.NotNullOrEmpty(passwordSHA256Hash, nameof(passwordSHA256Hash));
            Guard.NotNullOrWhiteSpace(preferredLanguage, nameof(preferredLanguage));
            Guard.NotNull(owinContext, nameof(owinContext));

            this.EmailAddress = emailAddress;
            this.PasswordSHA256Hash = passwordSHA256Hash;
            this.PreferredLanguage = preferredLanguage;
            this.OwinContext = owinContext;
        }

        public string EmailAddress { get; }
        [Secure]
        public byte[] PasswordSHA256Hash { get; }
        public string PreferredLanguage { get; }
        [Secure]
        public IOwinContext OwinContext { get; }
    }
}