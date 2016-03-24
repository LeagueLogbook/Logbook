using Logbook.Shared;
using Microsoft.Owin;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class AuthenticateCommand : ICommand<int>
    {
        public AuthenticateCommand(IOwinContext context)
        {
            Guard.NotNull(context, nameof(context));

            this.Context = context;
        }

        [Secure]
        public IOwinContext Context { get; }
    }
}