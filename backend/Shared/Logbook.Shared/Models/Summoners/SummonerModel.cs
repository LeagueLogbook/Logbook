using Logbook.Shared.Entities.Summoners;

namespace Logbook.Shared.Models.Summoners
{
    public class SummonerModel
    {
        public long Id { get; set; }
        public Region Region { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int ProfileIconId { get; set; }
    }
}