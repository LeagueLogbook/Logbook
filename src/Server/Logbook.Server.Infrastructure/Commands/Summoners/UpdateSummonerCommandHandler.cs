using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using NHibernate;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class UpdateSummonerCommandHandler : ICommandHandler<UpdateSummonerCommand, object>
    {
        private readonly ISession _session;
        private readonly ILeagueService _leagueService;

        public UpdateSummonerCommandHandler(ISession session, ILeagueService leagueService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(leagueService, nameof(leagueService));

            this._session = session;
            this._leagueService = leagueService;
        }

        public async Task<object> Execute(UpdateSummonerCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var summoner = this._session.Load<Summoner>(command.SummonerId);
            var riotSummoner = await this._leagueService.GetSummonerAsync(summoner.Region, summoner.RiotSummonerId);

            summoner.Name = riotSummoner.Name;
            summoner.Level = riotSummoner.Level;
            summoner.ProfileIconId = riotSummoner.ProfileIconId;

            return new object();
        }
    }
}