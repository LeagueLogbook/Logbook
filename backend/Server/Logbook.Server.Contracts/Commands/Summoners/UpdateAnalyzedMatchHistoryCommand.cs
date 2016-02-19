using System;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class UpdateAnalyzedMatchHistoryCommand : ICommand<object>
    {
        public UpdateAnalyzedMatchHistoryCommand(int summonerId, DateTime latestAnalyzedMatchTimeStamp, AnalyzedMatchHistory analyzedMatchHistory)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));
            Guard.NotInvalidDateTime(latestAnalyzedMatchTimeStamp, nameof(latestAnalyzedMatchTimeStamp));
            Guard.NotNull(analyzedMatchHistory, nameof(analyzedMatchHistory));

            this.SummonerId = summonerId;
            this.LatestAnalyzedMatchTimeStamp = latestAnalyzedMatchTimeStamp;
            this.AnalyzedMatchHistory = analyzedMatchHistory;
        }

        public int SummonerId { get; }
        public DateTime LatestAnalyzedMatchTimeStamp { get; }
        public AnalyzedMatchHistory AnalyzedMatchHistory { get; }
    }
}