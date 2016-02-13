using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;

namespace Logbook.Server.Contracts.Commands.CurrentGames
{
    public class GetCurrentGameCommand : ICommand<CurrentGame>
    {
        public GetCurrentGameCommand(Region region, long riotSummonerId)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotZeroOrNegative(riotSummonerId, nameof(riotSummonerId));

            this.Region = region;
            this.RiotSummonerId = riotSummonerId;
        }

        public Region Region { get; }
        public long RiotSummonerId { get; }
    }
}