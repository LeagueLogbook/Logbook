using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class AddMissingSummonersCommandHandler : ICommandHandler<AddMissingSummonersCommand, object>
    {
        private readonly ISession _session;
        private readonly ILeagueService _leagueService;

        public AddMissingSummonersCommandHandler(ISession session, ILeagueService leagueService)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(leagueService, nameof(leagueService));

            this._session = session;
            this._leagueService = leagueService;
        }

        public async Task<object> Execute(AddMissingSummonersCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var existingSummonerIds = this._session.Query<Summoner>()
                .Where(f => f.Region == command.Region && command.RiotSummonerIds.Contains(f.RiotSummonerId))
                .Select(f => f.RiotSummonerId)
                .ToList();

            var missingSummonerIds = command.RiotSummonerIds.Except(existingSummonerIds).ToList();

            if (missingSummonerIds.Any())
            {
                var summoners = await this._leagueService.GetSummonersAsync(command.Region, missingSummonerIds);
                foreach (var summoner in summoners)
                {
                    await scope.Execute(new AddNewSummonerCommand(summoner));
                }
            }

            return new object();
        }
    }
}