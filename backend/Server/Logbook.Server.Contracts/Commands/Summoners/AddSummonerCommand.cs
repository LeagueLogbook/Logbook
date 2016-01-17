using System.Collections.Generic;
using LiteGuard;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class AddSummonerCommand : ICommand<object>
    {
        public string UserId { get; }
        public Region Region { get; }
        public string SummonerName { get; }

        public AddSummonerCommand(string userId, Region region, string summonerName)
        {
            Guard.AgainstNullArgument(nameof(userId), userId);
            Guard.AgainstNullArgument(nameof(summonerName), summonerName);

            this.UserId = userId;
            this.Region = region;
            this.SummonerName = summonerName;
        }
    }
}