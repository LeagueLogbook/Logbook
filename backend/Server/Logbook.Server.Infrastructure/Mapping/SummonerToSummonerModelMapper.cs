using System.Threading.Tasks;
using Logbook.Server.Contracts.Mapping;
using Logbook.Shared;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models;
using Logbook.Shared.Models.Summoners;

namespace Logbook.Server.Infrastructure.Mapping
{
    public class SummonerToSummonerModelMapper : IMapper<Summoner, SummonerModel>
    {
        public Task<SummonerModel> MapAsync(Summoner source)
        {
            Guard.NotNull(source, nameof(source));

            var result = new SummonerModel
            {
                Id = source.RiotSummonerId,
                Region = source.Region,
                Name = source.Name,
                Level = source.Level,
                ProfileIconUri = source.ProfileIconUri
            };

            return Task.FromResult(result);
        }
    }
}