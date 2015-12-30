using LiteGuard;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class ResetPasswordCommand : ICommand<object>
    {
        public ResetPasswordCommand(string emailAddress)
        {
            Guard.AgainstNullArgument(nameof(emailAddress), emailAddress);

            this.EmailAddress = emailAddress;
        }

        public string EmailAddress { get; set; }
    }
}