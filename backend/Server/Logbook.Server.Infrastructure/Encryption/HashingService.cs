using System.Security.Cryptography;
using Logbook.Server.Contracts.Encryption;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class HashingService : IHashingService
    {
        private readonly SHA256 _sha256Hash;

        public HashingService()
        {
            this._sha256Hash = SHA256.Create();
        }

        public byte[] ComputeSHA256Hash(byte[] data)
        {
            return this._sha256Hash.ComputeHash(data);
        }
    }
}