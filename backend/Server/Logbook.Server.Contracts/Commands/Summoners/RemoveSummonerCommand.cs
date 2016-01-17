using LiteGuard;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class RemoveSummonerCommand : ICommand<object>
    {
        public string UserId { get; }
        public Region Region { get; }
        public long SummonerId { get; }

        public RemoveSummonerCommand(string userId, Region region, long summonerId)
        {
            Guard.AgainstNullArgument(nameof(userId), userId);

            this.UserId = userId;
            this.Region = region;
            this.SummonerId = summonerId;
        }
    }
}