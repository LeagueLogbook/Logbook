using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logbook.Server.Infrastructure.Emails;
using Logbook.Server.Infrastructure.Emails.Templates;
using Logbook.Server.Infrastructure.Social;

namespace Logbook.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var emailTemplateService = new EmailTemplateService();
            var template = new ConfirmEmailEmailTemplate
            {
                Url = "http://cs-ulm-danhae://",
                ValidDuration = TimeSpan.FromHours(2)
            };

            var email = emailTemplateService.GetTemplate(template);
        }
    }
}
