using System.Linq;
using Logbook.Shared.Entities.Budgets;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Logbook.Server.Infrastructure.Raven.Indexes
{
    public class Budgets_ByUserId : AbstractIndexCreationTask<Budget>
    {
        public Budgets_ByUserId()
        {
            this.Map = budgets => 
                from budget in budgets
                select new
                {
                    budget.ForUserId
                };

            this.Index(f => f.ForUserId, FieldIndexing.NotAnalyzed);
        }
    }
}