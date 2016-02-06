using System.Collections.Generic;

namespace Logbook.Shared.Entities.Games
{
    public class Team
    {
        public IList<BannedChampion> BannedChampions { get; set; }
        public IList<Participant> Participants { get; set; }
    }
}