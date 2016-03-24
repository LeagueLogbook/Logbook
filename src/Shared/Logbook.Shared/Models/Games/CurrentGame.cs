using System;
using Logbook.Shared.Entities.Summoners;

namespace Logbook.Shared.Models.Games
{
    public class CurrentGame
    {
        public long GameId { get; set; }
        public Region Region { get; set; }
        public GameMode GameMode { get; set; }
        public GameQueueType GameQueueType { get; set; }
        public GameType GameType { get; set; }
        public MapType MapType { get; set; }
        public DateTime GameStartTime { get; set; }
        public Team BlueTeam { get; set; }
        public Team PurpleTeam { get; set; }
    }
}