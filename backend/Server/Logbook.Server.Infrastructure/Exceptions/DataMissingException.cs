using System;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class DataMissingException : LogbookException
    {
        public DataMissingException()
            : base(ServerMessages.DataMissing)
        {
        }
    }
}