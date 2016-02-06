using Logbook.Shared.Entities.Summoners;

namespace Logbook.Shared.ControllerModels
{
    public class DeleteSummonerData
    {
        public Region Region { get; set; }
        public long SummonerId { get; set; }
    }
}