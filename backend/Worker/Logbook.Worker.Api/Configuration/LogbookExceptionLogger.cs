using System.Diagnostics;
using System.Web.Http.ExceptionHandling;
using Logbook.Server.Infrastructure;

namespace Logbook.Worker.Api.Configuration
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
            
        }
    }
}