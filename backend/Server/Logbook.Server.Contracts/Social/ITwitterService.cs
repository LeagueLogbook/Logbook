using System.Threading.Tasks;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Contracts.Social
{
    public interface ITwitterService : IService
    {
        Task<TwitterLoginUrl> GetLoginUrlAsync(string redirectUrl);

        Task<TwitterToken> ExchangeForToken(string payload, string oauthVerifier);

        Task<TwitterUser> GetMeAsync(TwitterToken token);
    }
}