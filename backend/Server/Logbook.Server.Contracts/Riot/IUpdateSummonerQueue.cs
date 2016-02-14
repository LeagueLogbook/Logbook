using System.Threading.Tasks;
using Logbook.Server.Contracts.Emails;

namespace Logbook.Server.Contracts.Riot
{
    public interface IUpdateSummonerQueue : IService
    {
        Task EnqueueSummonerAsync(int summonerId);
        Task<int?> TryDequeueSummonerAsync();

        Task RemoveAsync(int summonerId);
    }
}