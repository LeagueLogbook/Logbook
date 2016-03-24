using System.Security.Cryptography;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class SaltCombiner : ISaltCombiner
    {
        public byte[] Combine(byte[] salt, int iterationCount, string password)
        {
            Guard.NotNullOrEmpty(salt, nameof(salt));
            Guard.NotZeroOrNegative(iterationCount, nameof(iterationCount));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            using (var hasher = new Rfc2898DeriveBytes(password, salt, iterationCount))
            {
                return hasher.GetBytes(128);
            }
        }
    }
}