using FluentNHibernate.Mapping;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class FacebookAuthenticationKindMaps : SubclassMap<FacebookAuthenticationKind>
    {
        public FacebookAuthenticationKindMaps()
        {
            this.DiscriminatorValue("Facebook");

            this.Map(f => f.FacebookUserId)
                .Nullable();
        }
    }
}