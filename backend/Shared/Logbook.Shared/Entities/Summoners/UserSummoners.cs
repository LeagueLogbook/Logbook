using System.Collections.Generic;

namespace Logbook.Shared.Entities.Summoners
{
    public class UserSummoners : AggregateRoot
    {
        public static string CreateId(string forUserId) => $"{forUserId}/Summoners";

        public UserSummoners()
        {
            this.SummonerIds = new List<string>();
        }

        public string ForUserId { get; set; }
        public List<string> SummonerIds { get; set; } 
    }
}