using System;
using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class IncorrectPasswordException : LogbookException
    {
        public IncorrectPasswordException()
            : base(ServerMessages.IncorrectPassword)
        {
        }
    }
}