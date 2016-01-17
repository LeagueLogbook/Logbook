using System.Collections.Generic;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Mapping;

namespace Logbook.Server.Infrastructure.Mapping
{
    public static class MapperExtensions
    {
        public static async Task<IList<TTarget>> MapListAsync<TSource, TTarget>(this IMapper<TSource, TTarget> self, IEnumerable<TSource> list)
        {
            var result = new List<TTarget>();

            foreach (var item in list)
            {
                var mappedItem = await self.MapAsync(item);
                result.Add(mappedItem);
            }

            return result;
        }
    }
}