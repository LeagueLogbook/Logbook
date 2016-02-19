using System;
using System.Collections.Generic;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class AddMissingSummonersCommand : ICommand<object>
    {
        public AddMissingSummonersCommand(Region region, IList<long> riotSummonerIds)
        {
            Guard.NotInvalidEnum(region, nameof(region));
            Guard.NotNullOrEmpty(riotSummonerIds, nameof(riotSummonerIds));

            this.Region = region;
            this.RiotSummonerIds = riotSummonerIds;
        }

        public Region Region { get; }
        public IList<long> RiotSummonerIds { get; } 
    }
}