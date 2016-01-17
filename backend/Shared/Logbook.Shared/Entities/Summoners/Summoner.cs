namespace Logbook.Shared.Entities.Summoners
{
    public class Summoner : AggregateRoot
    {
        public static string CreateId(long riotSummonerId, Region region) => $"{region}/{riotSummonerId}";

        public long RiotSummonerId { get; set; }
        public string Name { get; set; }
        public Region Region { get; set; }
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
        Global
    }
}