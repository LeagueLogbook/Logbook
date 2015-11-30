using Logbook.Shared.Entities.Authentication;
using Raven.Client.Indexes;
using System.Linq;
using Raven.Abstractions.Indexing;

namespace Logbook.Server.Infrastructure.Raven.Indexes
{
    public class AuthenticationData_ByAllFields : AbstractIndexCreationTask<AuthenticationData, AuthenticationData_ByAllFields.Result>
    {
        public class Result
        {
            public string ForUserId { get; set; }
            public string LiveUserId { get; set; }
        }

        public AuthenticationData_ByAllFields()
        {
            this.Map = datas =>
                from data in datas
                let liveLogin = (LiveAuthenticationKind)data.Authentications.FirstOrDefault(f => f.Kind == AuthenticationKind.Live)
                select new Result
                {
                    ForUserId = data.ForUserId,
                    LiveUserId = liveLogin.LiveUserId
                };

            this.Index(f => f.ForUserId, FieldIndexing.NotAnalyzed);
            this.Index(f => f.LiveUserId, FieldIndexing.NotAnalyzed);
        }
    }
}