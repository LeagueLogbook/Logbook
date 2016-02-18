using System.Collections.Generic;

namespace Logbook.Shared.Entities.Summoners
{
    public class AnalyzedMatchHistory
    {
        public AnalyzedMatchHistory()
        {
            this.PlayedChampions = new List<ChampionWinrate>();
            this.Team = new List<Participant>();
            this.Enemies = new List<Participant>();
        }

        public IList<ChampionWinrate> PlayedChampions { get; set; }
        public IList<Participant> Team { get; set; } 
        public IList<Participant> Enemies { get; set; }
    }
}