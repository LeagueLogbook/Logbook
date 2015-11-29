using System.Linq;
using Logbook.Shared.Entities.Authentication;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Logbook.Server.Infrastructure.Raven.Indexes
{
    public class Users_ByEmailAddress : AbstractIndexCreationTask<User>
    {
        public Users_ByEmailAddress()
        {
            this.Map = users => 
                from user in users
                select new
                {
                    user.EmailAddress
                };

            this.Index(f => f.EmailAddress, FieldIndexing.NotAnalyzed);
        }
    }
}