using Logbook.Shared.Entities.Summoners;

namespace Logbook.Shared.Models
{
    public class SummonerModel
    {
        public long Id { get; set; }
        public Region Region { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string ProfileIconUri { get; set; }
    }
}