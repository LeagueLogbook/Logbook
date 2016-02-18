using System;
using System.Collections.Generic;

namespace Logbook.Shared.Entities.Summoners
{
    public class AnalyzedMatchHistory
    {
        public AnalyzedMatchHistory()
        {
            this.PlayedChampions = new List<ChampionWinrate>();
            this.TeamParticipants = new List<TeamParticipant>();
            this.Roles = new List<Role>();
        }

        public IList<ChampionWinrate> PlayedChampions { get; set; }
        public IList<TeamParticipant> TeamParticipants { get; set; } 
        public IList<Role> Roles { get; set; } 
    }

    public class ChampionWinrate
    {
        public ChampionWinrate()
        {
            this.KDAs = new List<double>();
            this.CSs = new List<int>();
            this.PlacedWards = new List<int>();
            this.GameLengths = new List<TimeSpan>();
        }

        public int ChampionId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public List<double> KDAs { get; set; } 
        public List<int> CSs { get; set; }
        public List<int> PlacedWards { get; set; }
        public List<TimeSpan> GameLengths { get; set; }
    }

    public class TeamParticipant
    {
        public long SummonerId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }

    public class Role
    {
        public Role()
        {
            this.KDAs = new List<double>();
            this.CSs = new List<int>();
            this.PlacedWards = new List<int>();
            this.GameLengths = new List<TimeSpan>();
        }

        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public List<double> KDAs { get; set; }
        public List<int> CSs { get; set; }
        public List<int> PlacedWards { get; set; } 
        public List<TimeSpan> GameLengths { get; set; } 
    }
}