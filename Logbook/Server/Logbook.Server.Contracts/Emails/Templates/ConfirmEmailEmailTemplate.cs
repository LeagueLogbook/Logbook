using System;
using System.Collections.Generic;

namespace Logbook.Server.Contracts.Emails.Templates
{
    public class ConfirmEmailEmailTemplate : IEmailTemplate
    {
        public string Url { get; set; }
        public TimeSpan ValidDuration { get; set; }

        public Dictionary<string, string> GetVariablesToReplace()
        {
            return new Dictionary<string, string>
            {
                ["Url"] = this.Url,
                ["ValidDuration"] = this.ValidDuration.ToString("%h")
            };
        }
    }
}