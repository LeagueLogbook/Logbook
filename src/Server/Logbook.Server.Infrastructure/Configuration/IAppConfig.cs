namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IAppConfig
    {
        bool InDebugHoldOnException { get; }
    }
}