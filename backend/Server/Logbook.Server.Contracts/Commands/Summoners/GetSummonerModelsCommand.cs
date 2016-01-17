using System.Collections.Generic;
using LiteGuard;
using Logbook.Shared.Models;

namespace Logbook.Server.Contracts.Commands.Summoners
{
    public class GetSummonerModelsCommand : ICommand<IList<SummonerModel>>
    {
        public string UserId { get; }

        public GetSummonerModelsCommand(string userId)
        {
            Guard.AgainstNullArgument(nameof(userId), userId);

            this.UserId = userId;
        }
    }
}