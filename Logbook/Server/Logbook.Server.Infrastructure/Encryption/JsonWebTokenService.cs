using System;
using JWT;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
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

        public string ValidateAndDecode(string jsonWebToken)
        {
            try
            {
                var decodedTokenAsJsonString = JsonWebToken.Decode(jsonWebToken, Config.AuthenticationKeyPhrase, verify: true);
                JObject json = JObject.Parse(decodedTokenAsJsonString);

                string userId = json.Value<string>("userId");

                return userId;
            }
            catch (SignatureVerificationException)
            {
                throw new JsonWebTokenTimedOutException();
            }
            catch (Exception)
            {
                throw new InvalidJsonWebTokenException();
            }
        }
    }
}