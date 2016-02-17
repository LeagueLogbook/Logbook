using System;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.CurrentGames;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.CurrentGames
{
    public class GetCurrentGameCommandHandler : ICommandHandler<GetCurrentGameCommand, CurrentGame>
    {
        private readonly ISession _session;
        private readonly ILeagueService _leagueService;

        public GetCurrentGameCommandHandler(ISession session, ILeagueService leagueService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(leagueService, nameof(leagueService));

            this._session = session;
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

            var existingSummoners = this._session.Query<Summoner>()
                .Where(f => f.Region == command.Region && summonerIds.Contains(f.RiotSummonerId))
                .ToList();

            foreach (var notExistingSummoner in summonerIds.Where(f => existingSummoners.Select(d => d.RiotSummonerId).Contains(f) == false))
            {
                var summoner = await this._leagueService.GetSummonerAsync(command.Region, notExistingSummoner);
                await scope.Execute(new AddNewSummonerCommand(summoner));
            }

            return game;
        }
    }
}