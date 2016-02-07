using LiteGuard;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class RemoveSummonerCommand : ICommand<object>
    {
        public int UserId { get; }
        public Region Region { get; }
        public long SummonerId { get; }

        public RemoveSummonerCommand(int userId, Region region, long summonerId)
        {
            this.UserId = userId;
            this.Region = region;
            this.SummonerId = summonerId;
        }
    }
}