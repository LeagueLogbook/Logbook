using Logbook.Shared;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class FinishRegistrationCommand : ICommand<object>
    {
        public FinishRegistrationCommand(string token)
        {
            Guard.NotNullOrWhiteSpace(token, nameof(token));

            this.Token = token;
        }

        [Secure]
        public string Token { get; } 
    }
}