using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    public class InvalidJsonWebTokenException : LogbookException
    {
        public InvalidJsonWebTokenException()
            : base("")
        {
        }
    }
}