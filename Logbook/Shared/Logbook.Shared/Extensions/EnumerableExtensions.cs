using System;
using System.Collections.Generic;
using System.Linq;

namespace Logbook.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Traverses the whole object specified by the <paramref name="childSelector"/>.
        /// </summary>
        /// <typeparam name="T">The type of items to traverse.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="childSelector">The child selector.</param>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var result = new HashSet<T>();

            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();

                result.Add(next);

                foreach (var child in childSelector(next).Where(f => result.Contains(f) == false))
                    stack.Push(child);
            }

            return result;
        }
    }
}