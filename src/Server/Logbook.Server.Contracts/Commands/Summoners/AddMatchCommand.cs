using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.MatchHistory;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class AddMatchCommand : ICommand<object>
    {
        public AddMatchCommand(int summonerId, PlayedMatch match)
        {
            Guard.NotZeroOrNegative(summonerId, nameof(summonerId));
            Guard.NotNull(match, nameof(match));

            this.SummonerId = summonerId;
            this.Match = match;
        }

        public int SummonerId { get; }
        public PlayedMatch Match { get; }
    }
}