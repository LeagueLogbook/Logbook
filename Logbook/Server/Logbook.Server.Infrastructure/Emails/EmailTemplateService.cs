using System;
using System.Collections.Generic;
using Logbook.Localization.Server;
using Logbook.Server.Contracts.Emails;
using Logbook.Shared;

namespace Logbook.Server.Infrastructure.Emails
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public Email GetTemplate(IEmailTemplate email)
        {
            var variables = email.GetVariablesToReplace();

            var template = this.ReadEmailTemplate(email.GetType());
            template.Subject = this.ReplaceVariables(template.Subject, variables);
            template.Body = this.ReplaceVariables(template.Body, variables);

            return template;
        }

        private string ReplaceVariables(string text, Dictionary<string, string> variables)
        {
            foreach (var pair in variables)
            {
                text = text.Replace($"@@{pair.Key}@@", pair.Value);
            }

            return text;
        }

        private Email ReadEmailTemplate(Type templateType)
        {
            string name = templateType.Name;

            if (templateType.Name.EndsWith(Constants.Email.TemplateSuffix))
                name = name.Substring(0, name.Length - Constants.Email.TemplateSuffix.Length);

            string subjectPropertyName = name + "_Subject";
            string bodyPropertyName = name + "_Body";

            string subject = (string)typeof (EmailTemplateMessages).GetProperty(subjectPropertyName).GetValue(null);
            string body = (string)typeof (EmailTemplateMessages).GetProperty(bodyPropertyName).GetValue(null);

            return new Email
            {
                Subject = subject,
                Body = body
            };
        }
    }
}