using System.Collections.Generic;
using FluentNHibernate.Mapping;
using Logbook.Server.Infrastructure.NHibernate.Extensions;
using Logbook.Server.Infrastructure.NHibernate.UserTypes;
using Logbook.Shared.Entities.Summoners;
using NHibernate.Type;

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
                .Index("idx_Region")
                .UniqueKey("RiotSummonerIdentity");

            this.Map(f => f.RiotSummonerId)
                .Not.Nullable()
                .Index("idx_RiotSummonerId")
                .UniqueKey("RiotSummonerIdentity");

            this.Map(f => f.Level)
                .Not.Nullable();

            this.Map(f => f.Name)
                .Not.Nullable();

            this.Map(f => f.ProfileIconId)
                .Not.Nullable();

            this.HasManyToMany(f => f.WatchedByUsers)
                .Table("UsersToWatchedSummoners")
                .ParentKeyColumn("SummonerId")
                .ChildKeyColumn("UserId")
                .Cascade.None()
                .Inverse()
                .LazyLoad();
            
            this.Map(f => f.LatestMatchTimeStamp)
                .CustomType<TimestampType>()
                .Nullable();

            this.Map(f => f.MatchIds)
                .CustomType<CompressedJson<ISet<long>>>()
                .Nullable();
        }
    }
}