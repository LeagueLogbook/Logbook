using System;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class JsonWebTokenTimedOutException : LogbookException
    {
        public JsonWebTokenTimedOutException()
            : base("")
        {
        }
    }
}