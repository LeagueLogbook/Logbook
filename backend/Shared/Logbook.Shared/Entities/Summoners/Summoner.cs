using System.Collections.Generic;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Shared.Entities.Summoners
{
    public class Summoner : AggregateRoot
    {
        public Summoner()
        {
            this.WatchedByUsers = new List<User>();
        }

        public virtual Region Region { get; set; }
        public virtual long RiotSummonerId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Level { get; set; }
        public virtual string ProfileIconUri { get; set; }
        public virtual IList<User> WatchedByUsers { get; set; } 
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