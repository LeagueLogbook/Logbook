using System.Collections.Generic;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class AddSummonerCommand : ICommand<object>
    {
        public AddSummonerCommand(int userId, Region region, string summonerName)
        {
            Guard.NotZeroOrNegative(userId, nameof(userId));
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotNullOrWhiteSpace(summonerName, nameof(summonerName));

            this.UserId = userId;
            this.Region = region;
            this.SummonerName = summonerName;
        }
        
        public int UserId { get; }
        public Region Region { get; }
        public string SummonerName { get; }
    }
}