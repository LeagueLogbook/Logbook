using JetBrains.Annotations;
using LiteGuard;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class RegisterCommand : ICommand<User>
    {
        public RegisterCommand([NotNull]string emailAddress, [NotNull]byte[] passwordMD5Hash, [NotNull]string preferredLanguage)
        {
            Guard.AgainstNullArgument(nameof(emailAddress), emailAddress);
            Guard.AgainstNullArgument(nameof(passwordMD5Hash), passwordMD5Hash);
            Guard.AgainstNullArgument(nameof(preferredLanguage), preferredLanguage);

            this.EmailAddress = emailAddress;
            this.PasswordMD5Hash = passwordMD5Hash;
            this.PreferredLanguage = preferredLanguage;
        }

        public string EmailAddress { get; }
        public byte[] PasswordMD5Hash { get; }
        public string PreferredLanguage { get; }
    }
}