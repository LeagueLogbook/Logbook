using FluentNHibernate.Mapping;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class AuthenticationKindBaseMaps : ClassMap<AuthenticationKindBase>
    {
        public AuthenticationKindBaseMaps()
        {
            this.Table("Authentications");

            this.Id(f => f.Id)
                .GeneratedBy.Identity();

            this.References(f => f.User)
                .Column("UserId")
                .Not.Nullable();
            
            this.DiscriminateSubClassesOnColumn("Kind").Not.Nullable();
        }
    }
}