using System.IO;
using System.Security.Cryptography;
using System.Text;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared;
using Newtonsoft.Json;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        public byte[] Encrypt<T>(T payload, string password)
        {
            Guard.NotNull(payload, nameof(payload));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            using (var algorithm = this.CreateAlgorithm(password))
            using (var stream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                string json = JsonConvert.SerializeObject(payload);
                byte[] data = Encoding.UTF8.GetBytes(json);

                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return stream.ToArray();
            }
        }

        public T Decrypt<T>(byte[] data, string password)
        {
            Guard.NotNullOrEmpty(data, nameof(data));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            using (var algorithm = this.CreateAlgorithm(password))
            using (var stream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                byte[] decryptedData = stream.ToArray();
                string json = Encoding.UTF8.GetString(decryptedData);

                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private SymmetricAlgorithm CreateAlgorithm(string password)
        {
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            if (password.Length < 8)
                password = password.PadRight(8);

            Rfc2898DeriveBytes passwordDeriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password));

            var rijndael = new RijndaelManaged();
            rijndael.Key = passwordDeriveBytes.GetBytes(32);
            rijndael.IV = passwordDeriveBytes.GetBytes(16);

            return rijndael;
        }
    }
}