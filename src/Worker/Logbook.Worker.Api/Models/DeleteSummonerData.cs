using Logbook.Shared.Entities.Summoners;

namespace Logbook.Worker.Api.Models
{
    public class DeleteSummonerData
    {
        public Region Region { get; set; }
        public long SummonerId { get; set; }
    }
}