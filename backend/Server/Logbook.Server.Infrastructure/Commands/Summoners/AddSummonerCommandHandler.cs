using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared.Entities.Summoners;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class AddSummonerCommandHandler : ICommandHandler<AddSummonerCommand, object>
    {
        private readonly ILeagueService _leagueService;
        private readonly IAsyncDocumentSession _documentSession;

        public AddSummonerCommandHandler(ILeagueService leagueService, IAsyncDocumentSession documentSession)
        {
            this._leagueService = leagueService;
            this._documentSession = documentSession;
        }

        public async Task<object> Execute(AddSummonerCommand command, ICommandScope scope)
        {
            var userSummoners = await scope.Execute(new GetSummonersCommand(command.UserId));
            var summoner = await this._leagueService.GetSummonerAsync(command.Region, command.SummonerName);

            if (summoner == null)
                throw new SummonerNotFoundException();

            var summonerId = Summoner.CreateId(summoner.RiotSummonerId, summoner.Region);

            bool userAlreadyHasSummoner = userSummoners.SummonerIds.Any(f => f == summonerId);

            if (userAlreadyHasSummoner == false)
            {
                userSummoners.SummonerIds.Add(summonerId);
            }

            var existing = await this._documentSession.LoadAsync<Summoner>(summonerId);
            if (existing == null)
            {
                await this._documentSession.StoreAsync(summoner);
            }

            return new object();
        }
    }
}