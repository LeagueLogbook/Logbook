namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IEmailTemplateConfig
    {
        string ConfirmEmailSender { get; }
        string PasswordResetEmailSender { get; }
    }
}