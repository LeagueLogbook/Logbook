using System.Collections;
using System.Collections.Generic;
using LiteGuard;
using Logbook.Shared.Entities.Budgets;

namespace Logbook.Server.Contracts.Commands.Budgets
{
    public class GetBudgetsCommand : ICommand<IList<Budget>>
    {
        public string ForUserId { get; }

        public GetBudgetsCommand(string forUserId)
        {
            Guard.AgainstNullArgument(nameof(forUserId), forUserId);

            this.ForUserId = forUserId;
        }
    }
}