using System;

namespace Logbook.Server.Infrastructure.Exceptions
{
    [Serializable]
    public class PasswordResetTimedOutException : LogbookException
    {
        public PasswordResetTimedOutException()
            : base("")
        {
            
        }
    }
}