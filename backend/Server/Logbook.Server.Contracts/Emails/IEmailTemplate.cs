using System.Collections.Generic;

namespace Logbook.Server.Contracts.Emails
{
    public interface IEmailTemplate
    {
        string Sender { get; }
        Dictionary<string, string> GetVariablesToReplace();
    }
}