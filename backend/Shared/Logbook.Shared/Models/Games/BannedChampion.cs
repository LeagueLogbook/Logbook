namespace Logbook.Shared.Models.Games
{
    public class BannedChampion
    {
        public long ChampionId { get; set; }
        public string ChampionName { get; set; }
        public int PickTurn { get; set; }
    }
}