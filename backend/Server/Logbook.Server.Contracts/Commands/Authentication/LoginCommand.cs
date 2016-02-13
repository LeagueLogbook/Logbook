using System;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class LoginCommand : ICommand<JsonWebToken>
    {
        public LoginCommand(string emailAddress, byte[] passwordSha256Hash)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Guard.NotNullOrEmpty(passwordSha256Hash, nameof(passwordSha256Hash));

            this.EmailAddress = emailAddress;
            this.PasswordSHA256Hash = passwordSha256Hash;
        }

        public string EmailAddress { get; }
        public byte[] PasswordSHA256Hash { get; }
    }
}