using System.Security.Cryptography;
using LiteGuard;
using Logbook.Server.Contracts.Encryption;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class SaltCombiner : ISaltCombiner
    {
        public byte[] Combine(byte[] salt, int iterationCount, string password)
        {
            Guard.AgainstNullArgument(nameof(salt), salt);
            Guard.AgainstNullArgument(nameof(password), password);

            using (var hasher = new Rfc2898DeriveBytes(password, salt, iterationCount))
            {
                return hasher.GetBytes(128);
            }
        }
    }
}