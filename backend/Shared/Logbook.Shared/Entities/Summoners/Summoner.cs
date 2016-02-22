using System;
using System.Collections.Generic;
using Logbook.Shared.Entities.Authentication;

namespace Logbook.Shared.Entities.Summoners
{
    public class Summoner : AggregateRoot
    {
        public Summoner()
        {
            this.WatchedByUsers = new List<User>();
            this.MatchIds = new HashSet<long>();
        }

        public virtual Region Region { get; set; }
        public virtual long RiotSummonerId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Level { get; set; }
        public virtual string ProfileIconUri { get; set; }
        public virtual IList<User> WatchedByUsers { get; set; } 
        public virtual DateTime? LatestMatchTimeStamp { get; set; }
        public virtual ISet<long> MatchIds { get; set; }
    }
}