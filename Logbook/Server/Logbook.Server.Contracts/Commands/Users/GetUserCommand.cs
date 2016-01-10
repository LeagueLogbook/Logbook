using LiteGuard;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Users
{
    public class GetUserCommand : ICommand<User>
    {
        public GetUserCommand(string userId)
        {
            Guard.AgainstNullArgument(nameof(userId), userId);

            this.UserId = userId;
        }

        public string UserId { get; }
    }
}