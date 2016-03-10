using Logbook.Server.Contracts.Encryption;
using Logbook.Shared;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class TwitterLoginCommand : ICommand<JsonWebToken>
    {
        public TwitterLoginCommand(string oAuthVerifier, string payload)
        {
            Guard.NotNullOrWhiteSpace(oAuthVerifier, nameof(oAuthVerifier));
            Guard.NotNullOrWhiteSpace(payload, nameof(payload));

            this.OAuthVerifier = oAuthVerifier;
            this.Payload = payload;
        }

        [Secure]
        public string OAuthVerifier { get; }
        [Secure]
        public string Payload { get; }
    }
}