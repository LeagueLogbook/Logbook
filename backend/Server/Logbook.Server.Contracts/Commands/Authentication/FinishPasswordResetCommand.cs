using Logbook.Shared;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class FinishPasswordResetCommand : ICommand<object>
    {
        public FinishPasswordResetCommand(string token)
        {
            Guard.NotNullOrWhiteSpace(token, nameof(token));

            this.Token = token;
        }

        public string Token { get; }
    }
}