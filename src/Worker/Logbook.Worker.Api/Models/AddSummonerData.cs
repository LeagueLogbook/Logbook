using Logbook.Shared.Entities.Summoners;

namespace Logbook.Worker.Api.Models
{
    public class AddSummonerData
    {
        public Region Region { get; set; }
        public string SummonerName { get; set; }
    }
}