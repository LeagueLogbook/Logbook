namespace Logbook.Shared.Models.Games
{
    public class Participant
    {
        public bool IsBot { get; set; }
        public long SummonerId { get; set; }
        public string SummonerName { get; set; }
        public int ProfileIconId { get; set; }

        public long ChampionId { get; set; }
        public string ChampionName { get; set; }

        public long SummonerSpell1Id { get; set; }
        public string SummonerSpell1Name { get; set; }

        public long SummonerSpell2Id { get; set; }
        public string SummonerSpell2Name { get; set; }
    }
}