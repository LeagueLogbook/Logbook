using System;
using JetBrains.Annotations;
using LiteGuard;
using Microsoft.Owin;

namespace Logbook.Server.Contracts.Commands.Authentication
{
    public class AuthenticateCommand : ICommand<string>
    {
        public AuthenticateCommand([NotNull]IOwinContext context)
        {
            Guard.AgainstNullArgument(nameof(context), context);

            this.Context = context;
        }

        public IOwinContext Context { get; }
    }
}