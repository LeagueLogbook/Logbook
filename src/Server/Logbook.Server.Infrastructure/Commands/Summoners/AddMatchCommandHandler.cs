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
    public class AddMatchCommandHandler : ICommandHandler<AddMatchCommand, object>
    {
        private readonly ISession _session;
        private readonly IMatchStorage _matchStorage;

        public AddMatchCommandHandler(ISession session, IMatchStorage matchStorage)
        {
            Guard.NotNull(session, nameof(session));
            Guard.NotNull(matchStorage, nameof(matchStorage));

            this._session = session;
            this._matchStorage = matchStorage;
        }

        public async Task<object> Execute(AddMatchCommand command, ICommandScope scope)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(scope, nameof(scope));

            var summonerIds = command.Match.BlueTeam.Participants
                .Select(f => f.SummonerId)
                .Union(command.Match.PurpleTeam.Participants.Select(f => f.SummonerId))
                .ToList();
            
            var summoners = this._session.Query<Summoner>()
                .Where(f => summonerIds.Contains(f.RiotSummonerId))
                .ToList();

            foreach (var summoner in summoners)
            {
                summoner.MatchIds.Add(command.Match.MatchId);
            }

            await this._matchStorage.SaveMatchAsync(command.Match);

            summoners.First(f => f.Id == command.SummonerId).LatestMatchTimeStamp = command.Match.CreationDate;

            return new object();
        }
    }
}