using System;
using System.Collections.Generic;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Emails.Templates
{
    public class ConfirmEmailEmailTemplate : IEmailTemplate
    {
        public string Url { get; set; }
        public string Sender => Config.EmailTemplate.ConfirmEmailSender;

        public Dictionary<string, string> GetVariablesToReplace()
        {
            return new Dictionary<string, string>
            {
                ["Url"] = this.Url,
            };
        }
    }
}