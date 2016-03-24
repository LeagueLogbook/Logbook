using System.Collections.Generic;

namespace Logbook.Shared.Models.MatchHistory
{
    public class Team
    {
        public bool Winner { get; set; }
        public IList<Participant> Participants { get; set; } 
    }
}