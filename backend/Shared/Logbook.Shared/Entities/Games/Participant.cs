namespace Logbook.Shared.Entities.Games
{
    public class Participant
    {
        public bool IsBot { get; set; }
        public long SummonerId { get; set; }
        public string SummonerName { get; set; }
        public string ProfileIconUri { get; set; }

        public int ChampionId { get; set; }
        public string ChampionName { get; set; }

        public int SummonerSpell1Id { get; set; }
        public string SummonerSpell1Name { get; set; }
        public string SummonerSpell1IconUri { get; set; }

        public int SummonerSpell2Id { get; set; }
        public string SummonerSpell2Name { get; set; }
        public string SummonerSpell2IconUri { get; set; }
    }
}