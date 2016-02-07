using FluentNHibernate.Mapping;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class MicrosoftAuthenticationKindMaps : SubclassMap<MicrosoftAuthenticationKind>
    {
        public MicrosoftAuthenticationKindMaps()
        {
            this.DiscriminatorValue("Microsoft");

            this.Map(f => f.MicrosoftUserId)
                .Nullable();
        }
    }
}