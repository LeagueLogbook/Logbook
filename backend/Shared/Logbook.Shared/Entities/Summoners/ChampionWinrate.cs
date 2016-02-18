using System.Collections.Generic;

namespace Logbook.Shared.Entities.Summoners
{
    public class ChampionWinrate
    {
        public ChampionWinrate()
        {
            this.Stats = new List<ChampionStats>();
        }

        public int ChampionId { get; set; }
        public IList<ChampionStats> Stats { get; set; } 
    }
}