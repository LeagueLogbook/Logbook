using LiteGuard;
using Logbook.Shared.Entities.Budgets;

namespace Logbook.Server.Contracts.Commands.Budgets
{
    public class CreateBudgetCommand : ICommand<Budget>
    {
        public CreateBudgetCommand(string budgetName, string forUserId)
        {
            Guard.AgainstNullArgument(nameof(budgetName), budgetName);
            Guard.AgainstNullArgument(nameof(forUserId), forUserId);

            this.BudgetName = budgetName;
            this.ForUserId = forUserId;
        }

        public string BudgetName { get; }
        public string ForUserId { get; }
    }
}