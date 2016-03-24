using FluentNHibernate.Mapping;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.NHibernate.Extensions
{
    public static class PropertyPartExtensions
    {
        public static PropertyPart MaxLength(this PropertyPart map)
        {
            Guard.NotNull(map, nameof(map));

            return map.Length(10000);
        }
    }
}