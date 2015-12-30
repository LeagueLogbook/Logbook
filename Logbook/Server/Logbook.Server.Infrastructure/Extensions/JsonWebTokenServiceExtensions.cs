using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Models;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class JsonWebTokenServiceExtensions
    {
        public static JsonWebToken GenerateForLogin(this IJsonWebTokenService self, string userId)
        {
            var payload = new ForLogin
            {
                UserId = userId
            };

            return self.Generate(payload, Config.LoginIsValidForDuration, Config.AuthenticationKeyPhrase);
        }

        public static string ValidateAndDecodeForLogin(this IJsonWebTokenService self, string jsonWebToken)
        {
            return self.ValidateAndDecode<ForLogin>(jsonWebToken, Config.AuthenticationKeyPhrase).UserId;
        }

        public static JsonWebToken GenerateForConfirmEmail(this IJsonWebTokenService self, string emailAddress, string preferredLanauge, byte[] passwordSHA256Hash)
        {
            var payload = new ForConfirmEmail
            {
                EmailAddress = emailAddress,
                PreferredLanguage =  preferredLanauge,
                PasswordSHA256Hash = passwordSHA256Hash
            };

            return self.Generate(payload, Config.ConfirmEmailIsValidForDuration, Config.ConfirmEmailKeyPhrase);
        }

        public static ForConfirmEmail ValidateAndDecodeForConfirmEmail(this IJsonWebTokenService self, string jsonWebToken)
        {
            return self.ValidateAndDecode<ForConfirmEmail>(jsonWebToken, Config.ConfirmEmailKeyPhrase);
        }

        public static JsonWebToken GenerateForPasswordReset(this IJsonWebTokenService self, string emailAddress)
        {
            var payload = new ForPasswordReset
            {
                EmailAddress = emailAddress
            };

            return self.Generate(payload, Config.PasswordResetIsValidForDuration, Config.PasswordResetKeyPhrase);
        }

        public static string ValidateAndDecodeForPasswordReset(this IJsonWebTokenService self, string jsonWebToken)
        {
            return self.ValidateAndDecode<ForPasswordReset>(jsonWebToken, Config.PasswordResetKeyPhrase).EmailAddress;
        }

        public class ForLogin
        {
            public string UserId { get; set; }
        }

        public class ForConfirmEmail
        {
            public string EmailAddress { get; set; }
            public string PreferredLanguage { get; set; }
            public byte[] PasswordSHA256Hash { get; set; }
        }

        public class ForPasswordReset
        {
            public string EmailAddress { get; set; }
        }
    }
}