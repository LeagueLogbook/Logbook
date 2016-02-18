using Logbook.Shared.Entities.Summoners;

namespace Logbook.Shared.Models.MatchHistory
{
    public class Participant
    {
        public long SummonerId { get; set; }
        public int ChampionId { get; set; }
        public long Assists { get; set; }
        public long Deaths { get; set; }
        public long Kills { get; set; }
        public Role Role { get; set; }
        public Lane Lane { get; set; }
        public long Creeps { get; set; }
        public long PlacedWards { get; set; }
        public long DestroyedWards { get; set; }
    }
}