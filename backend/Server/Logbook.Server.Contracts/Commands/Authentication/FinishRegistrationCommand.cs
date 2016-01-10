using LiteGuard;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class FinishRegistrationCommand : ICommand<object>
    {
        public FinishRegistrationCommand(string token)
        {
            Guard.AgainstNullArgument(nameof(token), token);

            this.Token = token;
        }

        public string Token { get; } 
    }
}