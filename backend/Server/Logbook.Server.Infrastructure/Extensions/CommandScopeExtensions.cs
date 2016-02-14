using System.Collections.Generic;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Commands;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class CommandScopeExtensions
    {
        public static async Task<IList<TTarget>> MapList<TSource, TTarget>(this ICommandScope scope, IList<TSource> sources)
        {
            Guard.NotNull(scope, nameof(scope));
            Guard.NotNull(sources, nameof(sources));

            var result = new List<TTarget>();

            foreach (var source in sources)
            {
                var model = await scope.Map<TSource, TTarget>(source);
                result.Add(model);
            }

            return result;
        }
    }
}