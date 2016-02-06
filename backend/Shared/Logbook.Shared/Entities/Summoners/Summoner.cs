using System.Collections.Generic;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Shared.Entities.Summoners
{
    public class Summoner : AggregateRoot
    {
        public Region Region { get; set; }
        public long RiotSummonerId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string ProfileIconUri { get; set; }
        public IList<User> WatchedByUsers { get; set; } 
    }

    public enum Region
    {
        Br,
        Eune,
        Euw,
        Na,
        Kr,
        Lan,
        Las,
        Oce,
        Ru,
        Tr,
    }
}