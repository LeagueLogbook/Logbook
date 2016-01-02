using LiteGuard;
using Microsoft.Owin;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class ResetPasswordCommand : ICommand<object>
    {
        public ResetPasswordCommand(string emailAddress, IOwinContext owinContext)
        {
            Guard.AgainstNullArgument(nameof(emailAddress), emailAddress);
            Guard.AgainstNullArgument(nameof(owinContext), owinContext);

            this.EmailAddress = emailAddress;
            this.OwinContext = owinContext;
        }

        public string EmailAddress { get; set; }
        public IOwinContext OwinContext { get; set; }
    }
}