using System.Diagnostics;
using System.Web.Http.ExceptionHandling;
using Anotar.NLog;

namespace Logbook.Server.Infrastructure.Api.Configuration
{
    public class LogbookExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// When overridden in a derived class, logs the exception synchronously.   
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
#if DEBUG
            if (Debugger.IsAttached && Config.InDebugHoldOnException)
                Debugger.Break();
#endif

            LogTo.ErrorException($"Unhandled exception. Returning 501 Internal Server Error. Catch block: {context.CatchBlock}", context.Exception);
        }
    }
}