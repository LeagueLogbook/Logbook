using System;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class SecretGeneratorExtensions
    {
        public static string GenerateString(this ISecretGenerator self, int length = 20)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotZeroOrNegative(length, nameof(length));

            return Convert.ToBase64String(self.Generate(length)).Substring(0, length);
        }
    }
}