using JetBrains.Annotations;
using LiteGuard;
using Logbook.Server.Contracts.Encryption;
using Logbook.Shared.Models;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class MicrosoftLoginCommand : ICommand<JsonWebToken>
    {
        public MicrosoftLoginCommand([NotNull]string code, [NotNull]string redirectUrl)
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