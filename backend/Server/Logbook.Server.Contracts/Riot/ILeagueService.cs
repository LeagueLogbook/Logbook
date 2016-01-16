using System.Threading.Tasks;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Server.Contracts.Riot
{
    public interface ILeagueService : IService
    {
        Task<Summoner> GetSummonerAsync(Region region, string name);
    }
}