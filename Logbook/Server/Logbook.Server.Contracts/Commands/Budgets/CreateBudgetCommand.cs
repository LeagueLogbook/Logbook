using LiteGuard;
using Logbook.Shared.Entities.Budgets;

namespace Logbook.Server.Contracts.Commands.Budgets
{
    public class CreateBudgetCommand : ICommand<Budget>
    {
        public CreateBudgetCommand(string budgetName, string currentUserId)
        {
            Guard.AgainstNullArgument(nameof(budgetName), budgetName);
            Guard.AgainstNullArgument(nameof(currentUserId), currentUserId);

            this.BudgetName = budgetName;
            this.CurrentUserId = currentUserId;
        }

        public string BudgetName { get; }
        public string CurrentUserId { get; }
    }
}