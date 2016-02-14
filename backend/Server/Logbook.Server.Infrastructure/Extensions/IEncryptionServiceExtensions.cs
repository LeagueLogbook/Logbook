﻿using System;
using System.Security.Policy;
using System.Web;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Server.Infrastructure.Exceptions;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class IEncryptionServiceExtensions
    {
        public static string Encrypt<T>(this IEncryptionService self, T payload, string password)
        {
            return self.Encrypt(payload, password).ToBase64UrlSafe();
        }

        public static T Decrypt<T>(this IEncryptionService self, string data, string password)
        {
            return self.Decrypt<T>(data.FromBase64UrlSafe(), password);
        }

        public static string GenerateForConfirmEmail(this IEncryptionService self, string emailAddress, string preferredLanauge, byte[] passwordSHA256Hash)
        {
            var payload = new ForConfirmEmail
            {
                EmailAddress = emailAddress,
                PreferredLanguage = preferredLanauge,
                PasswordSHA256Hash = passwordSHA256Hash,
                Timeout = DateTimeOffset.UtcNow.AddMinutes(Config.Security.ConfirmEmailIsValidForMinutes)
            };

            return Encrypt(self, payload, Config.Security.ConfirmEmailKeyPhrase);
        }

        public static ForConfirmEmail ValidateAndDecodeForConfirmEmail(this IEncryptionService self, string data)
        {
            var result = self.Decrypt<ForConfirmEmail>(data, Config.Security.ConfirmEmailKeyPhrase);

            if (DateTimeOffset.UtcNow > result.Timeout)
                throw new ConfirmEmailTimedOutException();

            return result;
        }

        public static string GenerateForPasswordReset(this IEncryptionService self, string emailAddress)
        {
            var payload = new ForPasswordReset
            {
                EmailAddress = emailAddress,
                Timeout = DateTimeOffset.UtcNow.AddMinutes(Config.Security.PasswordResetIsValidForMinutes)
            };

            return Encrypt(self, payload, Config.Security.PasswordResetKeyPhrase);
        }

        public static string ValidateAndDecodeForPasswordReset(this IEncryptionService self, string jsonWebToken)
        {
            var result = self.Decrypt<ForPasswordReset>(jsonWebToken, Config.Security.PasswordResetKeyPhrase);

            if (DateTimeOffset.UtcNow > result.Timeout)
                throw new PasswordResetTimedOutException();

            return result.EmailAddress;
        }

        public static string GenerateForTwitterLogin(this IEncryptionService self, string oauthToken, string oauthTokenSecret)
        {
            var payload = new ForTwitterLogin
            {
                OAuthToken = oauthToken,
                OAuthTokenSecret = oauthTokenSecret
            };

            return Encrypt(self, payload, Config.Security.TwitterLoginKeyPhrase);
        }

        public static ForTwitterLogin DecodeForTwitterLogin(this IEncryptionService self, string encrypted)
        {
            return self.Decrypt<ForTwitterLogin>(encrypted, Config.Security.TwitterLoginKeyPhrase);
        }

        public class ForConfirmEmail
        {
            public string EmailAddress { get; set; }
            public string PreferredLanguage { get; set; }
            public byte[] PasswordSHA256Hash { get; set; }
            public DateTimeOffset Timeout { get; set; }
        }

        public class ForPasswordReset
        {
            public string EmailAddress { get; set; }
            public DateTimeOffset Timeout { get; set; }
        }

        public class ForTwitterLogin
        {
            public string OAuthToken { get; set; }
            public string OAuthTokenSecret { get; set; }
        }
    }
}