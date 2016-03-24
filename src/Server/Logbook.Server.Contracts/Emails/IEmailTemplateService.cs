namespace Logbook.Server.Contracts.Emails
{
    public interface IEmailTemplateService : IService
    {
        Email GetTemplate(IEmailTemplate email);
    }
}