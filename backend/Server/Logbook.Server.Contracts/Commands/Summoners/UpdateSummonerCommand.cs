using System;
using Logbook.Shared;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class UpdateSummonerCommand : ICommand<object>
    {
        public UpdateSummonerCommand(int summonerId)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));

            this.SummonerId = summonerId;
        }

        public int SummonerId { get; }
    }
}