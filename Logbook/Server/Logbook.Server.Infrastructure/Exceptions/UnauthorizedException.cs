using System;
using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class UnauthorizedException : LogbookException
    {
        public UnauthorizedException()
            : base(ServerMessages.NoAuthenticationTokenGiven)
        {
        }
    }
}