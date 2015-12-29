using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Models;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class JsonWebTokenServiceExtensions
    {
        public static JsonWebToken GenerateForLogin(this IJsonWebTokenService jsonWebTokenService, string userId)
        {
            return jsonWebTokenService.Generate(new ForLogin {UserId = userId}, Config.LoginIsValidForDuration);
        }
        public static string ValidateAndDecodeForLogin(this IJsonWebTokenService jsonWebTokenService, string jsonWebToken)
        {
            return jsonWebTokenService.ValidateAndDecode<ForLogin>(jsonWebToken).UserId;
        }

        private class ForLogin
        {
            public string UserId { get; set; }
        }
    }
}