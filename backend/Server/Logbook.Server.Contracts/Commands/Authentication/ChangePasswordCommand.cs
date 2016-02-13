using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class ChangePasswordCommand : ICommand<object>
    {
        public ChangePasswordCommand(User user, byte[] newPasswordSHA256Hash)
        {
            Guard.NotNull(user, nameof(user));
            Guard.NotNullOrEmpty(newPasswordSHA256Hash, nameof(newPasswordSHA256Hash));

            this.User = user;
            this.NewPasswordSHA256Hash = newPasswordSHA256Hash;
        }

        public User User { get; }
        public byte[] NewPasswordSHA256Hash { get; }
    }
}