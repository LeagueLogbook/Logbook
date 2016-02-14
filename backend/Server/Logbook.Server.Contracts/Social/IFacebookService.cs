using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Social
{
    public interface IFacebookService : IService
    {
        Task<string> GetLoginUrlAsync(string redirectUrl);

        Task<string> ExchangeCodeForTokenAsync(string redirectUrl, string code);

        Task<FacebookUser> GetMeAsync(string token);
    }
}