using System;
using JWT;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Results;
using Metrics.Utils;
using Newtonsoft.Json.Linq;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        public string Generate(string userId)
        {
            var payload = new
            {
                userId = userId,
                iss = "Logbook",
                exp = DateTime.UtcNow.Add(Config.LoginIsValidForDuration).ToUnixTime()
            };

            return JsonWebToken.Encode(payload, Config.AuthenticationKeyPhrase, JwtHashAlgorithm.HS256);
        }

        public Result<string> ValidateAndDecode(string jsonWebToken)
        {
            try
            {
                var decodedTokenAsJsonString = JsonWebToken.Decode(jsonWebToken, Config.AuthenticationKeyPhrase, verify: true);
                dynamic json = JObject.Parse(decodedTokenAsJsonString);

                return Result.AsSuccess((string)json.userId);
            }
            catch (SignatureVerificationException)
            {
                return Result.AsError("Session timed out.");
            }
            catch (Exception)
            {
                return Result.AsError("Invalid session.");
            }
        }
    }
}