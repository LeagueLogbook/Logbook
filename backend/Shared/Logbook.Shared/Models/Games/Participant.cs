namespace Logbook.Shared.Models.Games
{
    public class Participant
    {
        public bool IsBot { get; set; }
        public long SummonerId { get; set; }
        public string SummonerName { get; set; }
        public string ProfileIconUri { get; set; }

        public long ChampionId { get; set; }
        public string ChampionName { get; set; }

        public long SummonerSpell1Id { get; set; }
        public string SummonerSpell1Name { get; set; }
        public string SummonerSpell1IconUri { get; set; }

        public long SummonerSpell2Id { get; set; }
        public string SummonerSpell2Name { get; set; }
        public string SummonerSpell2IconUri { get; set; }
    }
}