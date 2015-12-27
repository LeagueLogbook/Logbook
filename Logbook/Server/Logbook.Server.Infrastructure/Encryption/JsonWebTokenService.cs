using System;
using JWT;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared.Models;
using Metrics.Utils;
using Newtonsoft.Json.Linq;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        public AuthenticationToken Generate(string userId)
        {
            var expiresAt = DateTime.UtcNow.Add(Config.LoginIsValidForDuration).StripEverythingAfterSeconds();

            var payload = new
            {
                userId = userId,
                iss = "Logbook",
                exp = expiresAt.ToUnixTime()
            };

            var token = JsonWebToken.Encode(payload, Config.AuthenticationKeyPhrase, JwtHashAlgorithm.HS256);

            return new AuthenticationToken
            {
                ExpiresAt = expiresAt,
                Token = token
            };
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