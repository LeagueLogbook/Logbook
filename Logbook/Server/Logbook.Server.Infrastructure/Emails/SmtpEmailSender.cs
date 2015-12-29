using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Emails;

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
            var client = new SmtpClient(Config.SmtpHost, Config.SmtpPort)
            {
                UseDefaultCredentials = Config.SmtpUseDefaultCredentials,
                EnableSsl = Config.SmtpUseSsl,
            };
            
            if (client.UseDefaultCredentials == false)
                client.Credentials = new NetworkCredential(Config.SmtpUsername, Config.SmtpPassword);

            return client;
        }
        #endregion
    }
}