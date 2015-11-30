using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Social
{
    public interface ILiveService : IService
    {
        Task<string> ExchangeCodeForTokenAsync(string redirectUrl, string code);

        Task<LiveUser> GetMeAsync(string token);
    }
}