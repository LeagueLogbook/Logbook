using LiteGuard;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class RemoveSummonerCommand : ICommand<UserSummoners>
    {
        public string UserId { get; }
        public Region Region { get; }
        public string SummonerId { get; }

        public RemoveSummonerCommand(string userId, Region region, string summonerId)
        {
            Guard.AgainstNullArgument(nameof(userId), userId);
            Guard.AgainstNullArgument(nameof(summonerId), summonerId);

            this.UserId = userId;
            this.Region = region;
            this.SummonerId = summonerId;
        }
    }
}