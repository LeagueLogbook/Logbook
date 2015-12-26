using System.Net;
using Logbook.Localization.Server;

namespace Logbook.Server.Infrastructure.Exceptions
{
    public class InternalServerErrorException : LogbookException
    {
        public InternalServerErrorException()
            : base(ServerMessages.InternalServerError)
        {
        }
    }
}