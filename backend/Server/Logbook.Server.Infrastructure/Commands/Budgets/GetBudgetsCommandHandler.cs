using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Server.Contracts.Commands.Budgets;
using Logbook.Server.Infrastructure.Raven.Indexes;
using Logbook.Shared.Entities.Budgets;
using Raven.Client;

namespace Logbook.Server.Infrastructure.Commands.Budgets
{
    public class GetBudgetsCommandHandler : ICommandHandler<GetBudgetsCommand, IList<Budget>>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public GetBudgetsCommandHandler(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }

        public Task<IList<Budget>> Execute(GetBudgetsCommand command, ICommandScope scope)
        {
            return this._documentSession.Query<Budget, Budgets_ByUserId>()
                .Where(f => f.ForUserId == command.ForUserId)
                .ToListAsync();
        }
    }
}