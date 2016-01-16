using System.Collections.Generic;

namespace Logbook.Shared.Entities.Summoners
{
    public class UserSummoners : AggregateRoot
    {
        public static string CreateId(string forUserId) => $"{forUserId}/Summoners";

        public UserSummoners()
        {
            this.Summoners = new List<Summoner>();
        }

        public string ForUserId { get; set; }
        public List<Summoner> Summoners { get; set; } 
    }
}