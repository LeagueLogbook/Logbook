using System;
using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class UserNotFoundException : LogbookException
    {
        public UserNotFoundException()
            : base(ServerMessages.NoUserFound)
        {
        }
    }
}