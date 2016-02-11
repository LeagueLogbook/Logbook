using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;

namespace Logbook.Server.Contracts.Commands.CurrentGames
{
    public class GetCurrentGameCommand : ICommand<CurrentGame>
    {
        public Region Region { get; }
        public long RiotSummonerId { get; }

        public GetCurrentGameCommand(Region region, long riotSummonerId)
        {
            this.Region = region;
            this.RiotSummonerId = riotSummonerId;
        }
    }
}