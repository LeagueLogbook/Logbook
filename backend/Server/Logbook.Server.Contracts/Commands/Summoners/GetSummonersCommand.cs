using LiteGuard;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class GetSummonersCommand : ICommand<UserSummoners>
    {
        public string UserId { get; }

        public GetSummonersCommand(string userId)
        {
            Guard.AgainstNullArgument(nameof(userId), userId);

            this.UserId = userId;
        }
    }
}