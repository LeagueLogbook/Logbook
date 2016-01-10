using JetBrains.Annotations;
using LiteGuard;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class ChangePasswordCommand : ICommand<object>
    {
        public ChangePasswordCommand(User user, AuthenticationData authenticationData, byte[] newPasswordSHA256Hash)
        {
            Guard.AgainstNullArgument(nameof(user), user);
            Guard.AgainstNullArgument(nameof(authenticationData), authenticationData);
            Guard.AgainstNullArgument(nameof(newPasswordSHA256Hash), newPasswordSHA256Hash);

            this.User = user;
            this.AuthenticationData = authenticationData;
            this.NewPasswordSHA256Hash = newPasswordSHA256Hash;
        }

        public User User { get; }
        public AuthenticationData AuthenticationData { get; }
        public byte[] NewPasswordSHA256Hash { get; }
    }
}