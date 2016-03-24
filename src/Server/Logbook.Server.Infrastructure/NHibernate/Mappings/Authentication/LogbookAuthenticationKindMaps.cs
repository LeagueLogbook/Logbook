using FluentNHibernate.Mapping;
using Logbook.Server.Infrastructure.NHibernate.Extensions;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Infrastructure.NHibernate.Mappings.Authentication
{
    public class LogbookAuthenticationKindMaps : SubclassMap<LogbookAuthenticationKind>
    {
        public LogbookAuthenticationKindMaps()
        {
            this.DiscriminatorValue("Logbook");

            this.Map(f => f.Hash)
                .Nullable()
                .MaxLength();

            this.Map(f => f.IterationCount)
                .Nullable();

            this.Map(f => f.Salt)
                .Nullable()
                .MaxLength();
        }
    }
}