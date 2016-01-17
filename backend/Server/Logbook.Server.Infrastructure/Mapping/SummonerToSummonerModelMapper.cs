using System.Threading.Tasks;
using Logbook.Server.Contracts.Mapping;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models;

namespace Logbook.Server.Infrastructure.Mapping
{
    public class SummonerToSummonerModelMapper : IMapper<Summoner, SummonerModel>
    {
        public Task<SummonerModel> MapAsync(Summoner source)
        {
            var result = new SummonerModel
            {
                Id = source.RiotSummonerId,
                Region = source.Region,
                Name = source.Name
            };

            return Task.FromResult(result);
        }
    }
}