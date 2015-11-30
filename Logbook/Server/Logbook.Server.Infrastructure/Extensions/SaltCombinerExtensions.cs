using System;
using JetBrains.Annotations;
using Logbook.Server.Contracts.Encryption;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class SaltCombinerExtensions
    {
        /// <summary>
        /// Combines the specified <paramref name="password"/> with the specified <paramref name="salt"/>.
        /// </summary>
        /// <param name="self">The salt combiner.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterationCount">The iteration count.</param>
        /// <param name="password">The password.</param>
        [NotNull]
        public static byte[] Combine([NotNull]this ISaltCombiner self, [NotNull]byte[] salt, int iterationCount, [NotNull]byte[] password)
        {
            return self.Combine(salt, iterationCount, BitConverter.ToString(password));
        }
    }
}