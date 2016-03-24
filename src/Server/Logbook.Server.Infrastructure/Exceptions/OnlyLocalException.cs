using System;
using System.Runtime.Serialization;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class OnlyLocalException : LogbookException
    {
        public OnlyLocalException()
            : base(ServerMessages.OnlyLocal)
        {
        }
    }
}