using System.Collections.Generic;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class GetUserSummonersCommand : ICommand<IList<Summoner>>
    {
        public GetUserSummonersCommand(int userId)
        {
            Guard.NotZeroOrNegative(userId, nameof(userId));

            this.UserId = userId;
        }

        public int UserId { get; }
    }
}