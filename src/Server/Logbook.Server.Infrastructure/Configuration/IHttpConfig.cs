namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IHttpConfig
    {
        string Address { get; }
        bool CompressResponses { get; }
        bool EnableDebugRequestResponseLogging { get; }
        bool FormatResponses { get; }
    }
}