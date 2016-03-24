using FluentNHibernate.Mapping;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class TwitterAuthenticationKindMaps : SubclassMap<TwitterAuthenticationKind>
    {
        public TwitterAuthenticationKindMaps()
        {
            this.DiscriminatorValue("Twitter");

            this.Map(f => f.TwitterUserId)
                .Nullable();
        }
    }
}