namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IDatabaseConfig
    {
        string SqlServerConnectionString { get; }
        bool RecreateDatabase { get; }
        bool UpdateDatabase { get; }
    }
}