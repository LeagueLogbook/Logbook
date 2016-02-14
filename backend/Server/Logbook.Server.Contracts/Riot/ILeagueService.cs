using System.Threading.Tasks;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;

namespace Logbook.Server.Contracts.Riot
{
    public interface ILeagueService : IService
    {
        Task<Summoner> GetSummonerAsync(Region region, long riotSummonerId);

        Task<Summoner> GetSummonerAsync(Region region, string name);

        Task<CurrentGame> GetCurrentGameAsync(Region region, long riotSummonerId);
    }
}