using System;
using Logbook.Shared.Entities.Summoners;
using Logbook.Shared.Models.Games;

namespace Logbook.Shared.Models.MatchHistory
{
    public class PlayedMatch
    {
        public Region Region { get; set; }
        public long MatchId { get; set; }
        public DateTime CreationDate { get; set; }
        public TimeSpan Duration { get; set; } 
        public Team BlueTeam { get; set; }
        public Team PurpleTeam { get; set; }
    }
}