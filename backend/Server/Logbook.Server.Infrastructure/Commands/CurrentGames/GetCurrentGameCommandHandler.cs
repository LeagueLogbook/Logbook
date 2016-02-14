using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.CurrentGames;
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

        public Task<CurrentGame> Execute(GetCurrentGameCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            return this._leagueService.GetCurrentGameAsync(command.Region, command.RiotSummonerId);
        }
    }
}