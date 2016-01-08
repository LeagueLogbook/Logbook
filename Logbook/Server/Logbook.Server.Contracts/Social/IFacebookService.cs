using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Social
{
    public interface IFacebookService : IService
    {
        Task<string> GetLoginUrlAsync(string redirectUrl);

        Task<string> ExchangeCodeForTokenAsync(string redirectURl, string code);

        Task<FacebookUser> GetMeAsync(string token);
    }

    public interface ITwitterService : IService
    {
        Task<TwitterLoginUrl> GetLoginUrlAsync(string redirectUrl);

        Task<TwitterUser> GetMeAsync(string payload, string oauthVerifier);
    }
}