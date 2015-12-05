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
            public string MicrosoftUserId { get; set; }
        }

        public AuthenticationData_ByAllFields()
        {
            this.Map = datas =>
                from data in datas
                let microsoftLogin = (MicrosoftAuthenticationKind)data.Authentications.FirstOrDefault(f => f.Kind == AuthenticationKind.Microsoft)
                select new Result
                {
                    ForUserId = data.ForUserId,
                    MicrosoftUserId = microsoftLogin.MicrosoftUserId
                };

            this.Index(f => f.ForUserId, FieldIndexing.NotAnalyzed);
            this.Index(f => f.MicrosoftUserId, FieldIndexing.NotAnalyzed);
        }
    }
}