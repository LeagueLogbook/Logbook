using System.Collections.Generic;
using JetBrains.Annotations;

namespace Logbook.Server.Contracts.Emails
{
    public interface IEmailTemplate
    {
        string Sender { get; }
        Dictionary<string, string> GetVariablesToReplace();
    }
}