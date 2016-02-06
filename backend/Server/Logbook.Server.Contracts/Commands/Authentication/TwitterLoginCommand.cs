using LiteGuard;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Models;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class TwitterLoginCommand : ICommand<JsonWebToken>
    {
        public TwitterLoginCommand(string oAuthVerifier, string payload)
        {
            Guard.AgainstNullArgument(nameof(oAuthVerifier), oAuthVerifier);
            Guard.AgainstNullArgument(nameof(payload), payload);

            this.OAuthVerifier = oAuthVerifier;
            this.Payload = payload;
        }

        public string OAuthVerifier { get; }
        public string Payload { get; }
    }
}