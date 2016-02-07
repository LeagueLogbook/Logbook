using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class JsonWebTokenServiceExtensions
    {
        public static JsonWebToken GenerateForLogin(this IJsonWebTokenService self, int userId)
        {
            var payload = new ForLogin
            {
                UserId = userId
            };

            return self.Generate(payload, Config.LoginIsValidForDuration, Config.AuthenticationKeyPhrase);
        }

        public static int ValidateAndDecodeForLogin(this IJsonWebTokenService self, string jsonWebToken)
        {
            return self.ValidateAndDecode<ForLogin>(jsonWebToken, Config.AuthenticationKeyPhrase).UserId;
        }

        public class ForLogin
        {
            public int UserId { get; set; }
        }
    }
}