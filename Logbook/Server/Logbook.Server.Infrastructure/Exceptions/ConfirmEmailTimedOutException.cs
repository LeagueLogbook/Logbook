using System;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class ConfirmEmailTimedOutException : LogbookException
    {
        public ConfirmEmailTimedOutException()
            : base("")
        {
        }
    }
}