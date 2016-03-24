using System.Text;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class HashingServiceExtensions
    {
        public static byte[] ComputeSHA256Hash(this IHashingService self, string data)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotNullOrWhiteSpace(data, nameof(data));

            return self.ComputeSHA256Hash(Encoding.UTF8.GetBytes(data));
        }
    }
}