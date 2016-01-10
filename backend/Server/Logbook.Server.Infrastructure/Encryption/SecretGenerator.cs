using System.Security.Cryptography;
using Logbook.Server.Contracts.Encryption;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class SecretGenerator : ISecretGenerator
    {
        public byte[] Generate(int length = 128)
        {
            byte[] randomBytes = new byte[length];
            RandomNumberGenerator.Create().GetNonZeroBytes(randomBytes);

            return randomBytes;
        }
    }
}