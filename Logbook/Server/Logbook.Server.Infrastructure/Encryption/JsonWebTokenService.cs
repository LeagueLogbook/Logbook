﻿using System;
using JWT;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Exceptions;
using Logbook.Server.Infrastructure.Extensions;
using Logbook.Shared;
using Logbook.Shared.Models;
using Metrics.Utils;
using Newtonsoft.Json.Linq;
using JsonWebToken = Logbook.Shared.Models.JsonWebToken;

namespace Logbook.Server.Infrastructure.Encryption
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        public JsonWebToken Generate(string userId)
        {
            var expiresAt = DateTime.UtcNow.Add(Config.LoginIsValidForDuration).StripEverythingAfterSeconds();

            var payload = new
            {
                userId = userId,
                iss = Constants.Authentication.JWTIssuer,
                exp = expiresAt.ToUnixTime()
            };

            var token = JWT.JsonWebToken.Encode(payload, Config.AuthenticationKeyPhrase, JwtHashAlgorithm.HS256);

            return new JsonWebToken
            {
                ExpiresAt = expiresAt,
                Token = token
            };
        }

        public string ValidateAndDecode(string jsonWebToken)
        {
            try
            {
                var decodedTokenAsJsonString = JWT.JsonWebToken.Decode(jsonWebToken, Config.AuthenticationKeyPhrase, verify: true);
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