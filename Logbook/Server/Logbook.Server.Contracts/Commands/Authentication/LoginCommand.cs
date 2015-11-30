using System;
using JetBrains.Annotations;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class LoginCommand : ICommand<string>
    {
        public LoginCommand([NotNull]string emailAddress, [NotNull]byte[] passwordSha256Hash)
        {
            this.EmailAddress = emailAddress;
            this.PasswordSHA256Hash = passwordSha256Hash;
        }

        public string EmailAddress { get; }
        public byte[] PasswordSHA256Hash { get; }
    }
}