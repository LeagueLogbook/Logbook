using FluentNHibernate.Mapping;

namespace Logbook.Server.Infrastructure.NHibernate.Extensions
{
    public static class PropertyPartExtensions
    {
        public static PropertyPart MaxLength(this PropertyPart map)
        {
            return map.Length(10000);
        }
    }
}