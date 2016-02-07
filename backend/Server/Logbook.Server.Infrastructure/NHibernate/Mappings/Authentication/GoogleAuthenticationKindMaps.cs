using FluentNHibernate.Mapping;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class GoogleAuthenticationKindMaps : SubclassMap<GoogleAuthenticationKind>
    {
        public GoogleAuthenticationKindMaps()
        {
            this.DiscriminatorValue("Google");

            this.Map(f => f.GoogleUserId)
                .Nullable();
        }
    }
}