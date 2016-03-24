using Logbook.Shared;
using Microsoft.Owin;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class ResetPasswordCommand : ICommand<object>
    {
        public ResetPasswordCommand(string emailAddress, IOwinContext owinContext)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Guard.NotNull(owinContext, nameof(owinContext));

            this.EmailAddress = emailAddress;
            this.OwinContext = owinContext;
        }

        public string EmailAddress { get; }
        [Secure]
        public IOwinContext OwinContext { get; }
    }
}