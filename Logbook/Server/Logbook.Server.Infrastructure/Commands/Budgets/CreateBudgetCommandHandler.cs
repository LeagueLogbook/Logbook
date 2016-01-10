using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Budgets;
using Logbook.Shared.Entities.Budgets;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Budgets
{
    public class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, Budget>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public CreateBudgetCommandHandler(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }

        public async Task<Budget> Execute(CreateBudgetCommand command, ICommandScope scope)
        {
            var budget = new Budget
            {
                Name = command.BudgetName,
                ForUserId = command.ForUserId
            };

            await this._documentSession.StoreAsync(budget);

            return budget;
        }
    }
}