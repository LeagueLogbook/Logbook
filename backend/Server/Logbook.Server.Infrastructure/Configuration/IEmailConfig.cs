namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IEmailConfig
    {
        string SmtpHost { get; }
        int SmtpPort { get; }
        bool SmtpUseDefaultCredentials { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; }
        bool SmtpUseSsl { get; }
        string EmailQueueName { get; }
    }
}