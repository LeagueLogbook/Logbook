using LiteGuard;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class FinishPasswordResetCommand : ICommand<object>
    {
        public FinishPasswordResetCommand(string token)
        {
            Guard.AgainstNullArgument(nameof(token), token);

            this.Token = token;
        }

        public string Token { get; }
    }
}