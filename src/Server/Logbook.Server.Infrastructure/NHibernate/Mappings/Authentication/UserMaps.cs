using FluentNHibernate.Mapping;
using Logbook.Server.Infrastructure.NHibernate.Extensions;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class UserMaps : ClassMap<User>
    {
        public UserMaps()
        {
            this.Table("Users");

            this.Id(f => f.Id)
                .GeneratedBy.Identity();

            this.Map(f => f.EmailAddress)
                .Not.Nullable()
                .Length(200)
                .Unique()
                .Index("idx_EmailAddress");

            this.Map(f => f.PreferredLanguage)
                .Nullable()
                .Length(20);

            this.HasMany(f => f.Authentications)
                .KeyColumn("UserId")
                .Not.Inverse()
                .Cascade.AllDeleteOrphan()
                .LazyLoad();

            this.HasManyToMany(f => f.WatchSummoners)
                .Table("UsersToWatchedSummoners")
                .ParentKeyColumn("UserId")
                .ChildKeyColumn("SummonerId")
                .Cascade.None()
                .LazyLoad();
        }
    }
}