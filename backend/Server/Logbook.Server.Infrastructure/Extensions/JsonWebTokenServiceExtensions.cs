using System;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class JsonWebTokenServiceExtensions
    {
        public static JsonWebToken GenerateForLogin(this IJsonWebTokenService self, int userId)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotZeroOrNegative(userId, nameof(userId));

            var payload = new ForLogin
            {
                UserId = userId
            };

            return self.Generate(payload, TimeSpan.FromMinutes(Config.Security.LoginIsValidForMinutes), Config.Security.AuthenticationKeyPhrase);
        }

        public static int ValidateAndDecodeForLogin(this IJsonWebTokenService self, string jsonWebToken)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotNullOrWhiteSpace(jsonWebToken, nameof(jsonWebToken));

            return self.ValidateAndDecode<ForLogin>(jsonWebToken, Config.Security.AuthenticationKeyPhrase).UserId;
        }

        public class ForLogin
        {
            public int UserId { get; set; }
        }
    }
}