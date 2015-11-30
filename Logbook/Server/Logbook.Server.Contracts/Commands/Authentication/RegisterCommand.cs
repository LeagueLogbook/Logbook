using JetBrains.Annotations;
using LiteGuard;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class RegisterCommand : ICommand<User>
    {
        public RegisterCommand([NotNull]string emailAddress, [NotNull]byte[] passwordSHA256Hash, [NotNull]string preferredLanguage)
        {
            Guard.AgainstNullArgument(nameof(emailAddress), emailAddress);
            Guard.AgainstNullArgument(nameof(passwordSHA256Hash), passwordSHA256Hash);
            Guard.AgainstNullArgument(nameof(preferredLanguage), preferredLanguage);

            this.EmailAddress = emailAddress;
            this.PasswordSHA256Hash = passwordSHA256Hash;
            this.PreferredLanguage = preferredLanguage;
        }

        public string EmailAddress { get; }
        public byte[] PasswordSHA256Hash { get; }
        public string PreferredLanguage { get; }
    }
}