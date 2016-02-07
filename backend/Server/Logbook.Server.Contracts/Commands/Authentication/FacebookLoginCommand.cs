using LiteGuard;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class FacebookLoginCommand : ICommand<JsonWebToken>
    {
        public FacebookLoginCommand(string code, string redirectUrl)
        {
            Guard.AgainstNullArgument(nameof(code), code);
            Guard.AgainstNullArgument(nameof(redirectUrl), redirectUrl);

            this.Code = code;
            this.RedirectUrl = redirectUrl;
        }

        public string Code { get; }
        public string RedirectUrl { get; }
    }
}