namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IRiotConfig
    {
        string RiotApiKey { get; }
        int RiotApiRateLimitPer10Seconds { get; }
        int RiotApiRateLimitPer10Minutes { get; }
        int UpdateSummonersEveryMinutes { get; }
    }
}