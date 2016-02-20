using System;
using System.Threading.Tasks;

namespace Logbook.Server.Contracts.Riot
{
    public interface IAnalyzeSummonerMatchHistoryQueue : IService
    {
        Task EnqueueSummonerAsync(int summonerId);
        Task<int?> TryDequeueSummonerAsync();
        Task RequestMoreTimeToProcess(int summonerId, TimeSpan moreTime);

        Task RemoveAsync(int summonerId);
        Task TryAgainLaterAsync(int summonerId);
    }
}