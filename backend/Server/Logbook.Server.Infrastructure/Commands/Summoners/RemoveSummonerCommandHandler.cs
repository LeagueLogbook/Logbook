using System;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Summoners;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Infrastructure.Commands.Summoners
{
    public class RemoveSummonerCommandHandler : ICommandHandler<RemoveSummonerCommand, object>
    {
        public async Task<object> Execute(RemoveSummonerCommand command, ICommandScope scope)
        {
            var userSummoners = await scope.Execute(new GetSummonersCommand(command.UserId));

            var summonerId = Summoner.CreateId(command.SummonerId, command.Region);
            userSummoners.SummonerIds.Remove(summonerId);

            return new object();
        }
    }
}