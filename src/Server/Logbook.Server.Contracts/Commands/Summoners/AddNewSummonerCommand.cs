using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class AddNewSummonerCommand : ICommand<object>
    {
        public AddNewSummonerCommand(Summoner summoner)
        {
            Guard.NotNull(summoner, nameof(summoner));

            this.Summoner = summoner;
        }

        public Summoner Summoner { get; }
    }
}