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
            public string FacebookUserId { get; set; }
        }

        public AuthenticationData_ByAllFields()
        {
            this.Map = datas =>
                from data in datas
                let microsoftLogin = (MicrosoftAuthenticationKind)data.Authentications.FirstOrDefault(f => f.Kind == AuthenticationKind.Microsoft)
                let facebookLogin = (FacebookAuthenticationKind)data.Authentications.FirstOrDefault(f => f.Kind == AuthenticationKind.Facebook)
                select new Result
                {
                    ForUserId = data.ForUserId,
                    MicrosoftUserId = microsoftLogin.MicrosoftUserId,
                    FacebookUserId = facebookLogin.FacebookUserId
                };

            this.Index(f => f.ForUserId, FieldIndexing.NotAnalyzed);
            this.Index(f => f.MicrosoftUserId, FieldIndexing.NotAnalyzed);
            this.Index(f => f.FacebookUserId, FieldIndexing.NotAnalyzed);
        }
    }
}