using System;
using System.Runtime.Serialization;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class SummonerNotFoundException : LogbookException
    {
        public SummonerNotFoundException()
            : base("")
        {
        }
    }
}