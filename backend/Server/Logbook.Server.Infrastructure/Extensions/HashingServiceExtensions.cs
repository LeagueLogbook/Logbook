using System.Text;
using Logbook.Server.Contracts.Encryption;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class HashingServiceExtensions
    {
        public static byte[] ComputeSHA256Hash(this IHashingService self, string data)
        {
            return self.ComputeSHA256Hash(Encoding.UTF8.GetBytes(data));
        }
    }
}