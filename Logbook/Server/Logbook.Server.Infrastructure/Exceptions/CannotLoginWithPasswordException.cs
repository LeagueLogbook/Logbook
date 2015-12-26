using System;
using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class CannotLoginWithPasswordException : LogbookException
    {
        public CannotLoginWithPasswordException()
            : base(ServerMessages.CannotLoginWithPassword)
        {
        }
    }
}