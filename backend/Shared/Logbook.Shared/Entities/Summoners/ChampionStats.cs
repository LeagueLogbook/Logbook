using System;

namespace Logbook.Shared.Entities.Summoners
{
    public class ChampionStats
    {
        public bool Win { get; set; }
        public long Kills { get; set; }
        public long Deaths { get; set; }
        public long Assists { get; set; }
        public long Creeps { get; set; }
        public long PlacedWards { get; set; }
        public long DestroyedWards { get; set; }
        public TimeSpan GameLength { get; set; }
        public Role Role { get; set; }
        public Lane Lane { get; set; }
    }
}