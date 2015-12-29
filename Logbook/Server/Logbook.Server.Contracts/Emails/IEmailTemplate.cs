using System.Collections.Generic;

namespace Logbook.Server.Contracts.Emails
{
    public interface IEmailTemplate
    {
        Dictionary<string, string> GetVariablesToReplace();
    }
}