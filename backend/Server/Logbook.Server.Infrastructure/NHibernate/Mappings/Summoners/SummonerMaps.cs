using FluentNHibernate.Mapping;
using Logbook.Server.Infrastructure.NHibernate.Extensions;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Summoners
{
    public class SummonerMaps : ClassMap<Summoner>
    {
        public SummonerMaps()
        {
            this.Table("Summoners");

            this.Id(f => f.Id)
                .GeneratedBy.Identity();

            this.Map(f => f.Region)
                .Not.Nullable()
                .Index("idx_Region");

            this.Map(f => f.RiotSummonerId)
                .Not.Nullable()
                .Index("idx_RiotSummonerId");

            this.Map(f => f.Level)
                .Not.Nullable();

            this.Map(f => f.Name)
                .Not.Nullable();

            this.Map(f => f.ProfileIconUri)
                .MaxLength()
                .Nullable();

            this.HasManyToMany(f => f.WatchedByUsers)
                .Table("UsersToWatchedSummoners")
                .ParentKeyColumn("SummonerId")
                .ChildKeyColumn("UserId")
                .Cascade.None()
                .LazyLoad();
        }
    }
}