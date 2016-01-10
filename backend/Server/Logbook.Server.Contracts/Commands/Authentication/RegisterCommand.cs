using JetBrains.Annotations;
using LiteGuard;
using Logbook.Shared.Entities.Authentication;
using Microsoft.Owin;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class RegisterCommand : ICommand<object>
    {
        public RegisterCommand([NotNull]string emailAddress, [NotNull]byte[] passwordSHA256Hash, [NotNull]string preferredLanguage, [NotNull]IOwinContext owinContext)
        {
            Guard.AgainstNullArgument(nameof(emailAddress), emailAddress);
            Guard.AgainstNullArgument(nameof(passwordSHA256Hash), passwordSHA256Hash);
            Guard.AgainstNullArgument(nameof(preferredLanguage), preferredLanguage);
            Guard.AgainstNullArgument(nameof(owinContext), owinContext);

            this.EmailAddress = emailAddress;
            this.PasswordSHA256Hash = passwordSHA256Hash;
            this.PreferredLanguage = preferredLanguage;
            this.OwinContext = owinContext;
        }

        public string EmailAddress { get; }
        public byte[] PasswordSHA256Hash { get; }
        public string PreferredLanguage { get; }
        public IOwinContext OwinContext { get; }
    }
}