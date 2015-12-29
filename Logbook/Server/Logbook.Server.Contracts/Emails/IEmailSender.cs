using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Emails
{
    public interface IEmailSender : IService
    {
        Task SendMailAsync(Email email);
    }
}