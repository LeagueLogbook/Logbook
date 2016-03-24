using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class RemoveSummonerCommand : ICommand<object>
    {
        public RemoveSummonerCommand(int userId, Region region, long summonerId)
        {
            Guard.NotZeroOrNegative(userId, nameof(userId));
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            this.UserId = userId;
            this.Region = region;
            this.SummonerId = summonerId;
        }

        public int UserId { get; }
        public Region Region { get; }
        public long SummonerId { get; }
    }
}