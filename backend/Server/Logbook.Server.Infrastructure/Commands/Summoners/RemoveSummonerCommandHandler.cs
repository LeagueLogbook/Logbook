using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class RemoveSummonerCommandHandler : ICommandHandler<RemoveSummonerCommand, UserSummoners>
    {
        public async Task<UserSummoners> Execute(RemoveSummonerCommand command, ICommandScope scope)
        {
            var userSummoners = await scope.Execute(new GetSummonersCommand(command.UserId));

            var summoner = userSummoners.Summoners.FirstOrDefault(f => f.Id == command.SummonerId && f.Region == command.Region);
            if (summoner != null)
            {
                userSummoners.Summoners.Remove(summoner);
            }

            return userSummoners;
        }
    }
}