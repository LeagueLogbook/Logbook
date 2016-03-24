using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Entities.Summoners;
using NHibernate;
using NHibernate.Linq;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class AddSummonerCommandHandler : ICommandHandler<AddSummonerCommand, object>
    {
        private readonly ILeagueService _leagueService;
        private readonly ISession _session;

        public AddSummonerCommandHandler(ILeagueService leagueService, ISession session)
        {
            Guard.NotNull(leagueService, nameof(leagueService));
            Guard.NotNull(session, nameof(session));

            this._leagueService = leagueService;
            this._session = session;
        }

        public async Task<object> Execute(AddSummonerCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var summoner = await this._leagueService.GetSummonerAsync(command.Region, command.SummonerName);

            if (summoner == null)
                throw new SummonerNotFoundException();

            var user = this._session.Get<User>(command.UserId);

            if (user == null)
                throw new UserNotFoundException();
            
            bool userAlreadyHasSummoner = user.WatchSummoners.Any(f => f.RiotSummonerId == summoner.RiotSummonerId && f.Region == summoner.Region);

            if (userAlreadyHasSummoner == false)
            {
                var existingSummoner = this._session.Query<Summoner>()
                    .FirstOrDefault(f => f.RiotSummonerId == summoner.RiotSummonerId && f.Region == summoner.Region);

                if (existingSummoner == null)
                {
                    existingSummoner = summoner;
                    await scope.Execute(new AddNewSummonerCommand(existingSummoner));
                }

                existingSummoner.WatchedByUsers.Add(user);
                user.WatchSummoners.Add(existingSummoner);
            }
            
            return new object();
        }
    }
}