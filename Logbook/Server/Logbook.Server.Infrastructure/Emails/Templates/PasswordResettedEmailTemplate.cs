using System.Collections.Generic;
using Logbook.Server.Contracts.Emails;

namespace Logbook.Server.Infrastructure.Emails.Templates
{
    public class PasswordResettedEmailTemplate : IEmailTemplate
    {
        public string NewPassword { get; set; }
        public string Sender => Config.PasswordResetEmailSender;

        public Dictionary<string, string> GetVariablesToReplace()
        {
            return new Dictionary<string, string>
            {
                ["NewPassword"] = this.NewPassword
            };
        }
    }
}