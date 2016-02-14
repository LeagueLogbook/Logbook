using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Emails;
using Logbook.Server.Infrastructure.Configuration;

namespace Logbook.Server.Infrastructure.Emails
{
    public class SmtpEmailSender : IEmailSender
    {
        #region Implementation of IEmailSender
        public async Task SendMailAsync(Email email)
        {
            using (var mailMessage = this.ToMailMessage(email))
            using (var client = this.CreateSmtpClient())
            {
                await client.SendMailAsync(mailMessage);
            }
        }
        #endregion

        #region Private Methods
        private MailMessage ToMailMessage(Email email)
        {
            return new MailMessage(email.Sender, email.Receiver, email.Subject, email.Body);
        }
        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient(Config.Email.SmtpHost, Config.Email.SmtpPort)
            {
                UseDefaultCredentials = Config.Email.SmtpUseDefaultCredentials,
                EnableSsl = Config.Email.SmtpUseSsl,
            };
            
            if (client.UseDefaultCredentials == false)
                client.Credentials = new NetworkCredential(Config.Email.SmtpUsername, Config.Email.SmtpPassword);

            return client;
        }
        #endregion
    }
}