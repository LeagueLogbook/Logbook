using System.Collections.Generic;
using System.Threading.Tasks;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.MatchHistory;

namespace Logbook.Server.Contracts.Riot
{
    public interface IMatchStorage : IService
    {
        Task SaveMatchAsync(PlayedMatch match);
        Task<IList<PlayedMatch>> GetMatchesAsync(Region region, IList<long> matchIds);
    }
}