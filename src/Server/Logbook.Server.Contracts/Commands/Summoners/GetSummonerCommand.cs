using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class GetSummonerCommand : ICommand<Summoner>
    {
        public GetSummonerCommand(int summonerId)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            this.SummonerId = summonerId;
        }

        public int SummonerId { get; }
    }
}