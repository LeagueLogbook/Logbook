using System;
using Logbook.Server.Contracts.Encryption;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class SecretGeneratorExtensions
    {
        public static string GenerateString(this ISecretGenerator self, int length = 20)
        {
            return Convert.ToBase64String(self.Generate(length)).Substring(0, length);
        }
    }
}