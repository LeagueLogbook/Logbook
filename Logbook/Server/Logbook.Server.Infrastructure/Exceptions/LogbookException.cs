using System;
using System.Net;
using System.Runtime.Serialization;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class LogbookException : Exception
    {
        public LogbookException(string message)
            : base(message)
        {
        }

        public LogbookException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LogbookException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}