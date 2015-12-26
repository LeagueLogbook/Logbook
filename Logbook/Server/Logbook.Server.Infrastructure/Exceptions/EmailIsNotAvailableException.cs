using System;
using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class EmailIsNotAvailableException : LogbookException
    {
        public EmailIsNotAvailableException()
            : base(ServerMessages.EmailIsNotAvailable)
        {
        }
    }
}