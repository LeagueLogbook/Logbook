using System;
using System.Collections.Generic;
using Logbook.Server.Contracts.Emails;

namespace Logbook.Server.Infrastructure.Emails.Templates
{
    public class ResetPasswordEmailTemplate : IEmailTemplate
    {
        public string Url { get; set; }
        public string Sender => Config.PasswordResetEmailSender;

        public Dictionary<string, string> GetVariablesToReplace()
        {
            return new Dictionary<string, string>
            {
                ["Url"] = this.Url,
            };
        }
    }
}