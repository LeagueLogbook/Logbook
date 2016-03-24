using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.CurrentGames;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Shared;
using Logbook.Shared.Models.Games;

namespace Logbook.Server.Infrastructure.Commands.CurrentGames
{
    public class GetCurrentGameCommandHandler : ICommandHandler<GetCurrentGameCommand, CurrentGame>
    {
        private readonly ILeagueService _leagueService;

        public GetCurrentGameCommandHandler(ILeagueService leagueService)
        {
            Guard.NotNull(leagueService, nameof(leagueService));

            this._leagueService = leagueService;
        }

        public async Task<CurrentGame> Execute(GetCurrentGameCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var game = await this._leagueService.GetCurrentGameAsync(command.Region, command.RiotSummonerId);

            if (game == null)
                return null;

            var summonerIds = game.BlueTeam.Participants
                .Select(f => f.SummonerId)
                .Concat(game.PurpleTeam.Participants
                    .Select(f => f.SummonerId))
                .ToArray();

            await scope.Execute(new AddMissingSummonersCommand(command.Region, summonerIds));

            return game;
        }
    }
}