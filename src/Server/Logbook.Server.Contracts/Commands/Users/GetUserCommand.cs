using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Contracts.Commands.Users
{
    public class GetUserCommand : ICommand<User>
    {
        public GetUserCommand(int userId)
        {
            Guard.NotZeroOrNegative(userId, nameof(userId));

            this.UserId = userId;
        }

        public int UserId { get; }
    }
}