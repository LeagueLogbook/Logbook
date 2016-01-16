using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Server.Contracts.Riot;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class AddSummonerCommandHandler : ICommandHandler<AddSummonerCommand, UserSummoners>
    {
        private readonly ILeagueService _leagueService;

        public AddSummonerCommandHandler(ILeagueService leagueService)
        {
            this._leagueService = leagueService;
        }

        public async Task<UserSummoners> Execute(AddSummonerCommand command, ICommandScope scope)
        {
            var userSummoners = await scope.Execute(new GetSummonersCommand(command.UserId));
            var summoner = await this._leagueService.GetSummonerAsync(command.Region, command.SummonerName);

            if (summoner == null)
                throw new SummonerNotFoundException();

            bool userAlreadyHasSummoner = userSummoners.Summoners.Any(f => f.Id == summoner.Id && f.Region == summoner.Region);

            if (userAlreadyHasSummoner == false)
            {
                userSummoners.Summoners.Add(summoner);
            }

            return userSummoners;
        }
    }
}