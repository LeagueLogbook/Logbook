using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;
using Logbook.Shared.Models.MatchHistory;

namespace Logbook.Server.Contracts.Riot
{
    public interface ILeagueService : IService
    {
        Task<Summoner> GetSummonerAsync(Region region, long riotSummonerId);

        Task<Summoner> GetSummonerAsync(Region region, string name);

        Task<CurrentGame> GetCurrentGameAsync(Region region, long riotSummonerId);

        Task<List<long>> GetMatchHistory(Region region, long summonerId, DateTime? latestCheckedMatchTimeStamp);

        Task<PlayedMatch> GetMatch(Region region, long matchId);
    }
}