using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Emails
{
    public interface IEmailQueue : IService
    {
        Task EnqueueMailAsync(Email email);
        Task<Email> TryDequeueMailAsync();

        Task RemoveAsync(Email email);
        Task TryAgainLaterAsync(Email email);
    }
}