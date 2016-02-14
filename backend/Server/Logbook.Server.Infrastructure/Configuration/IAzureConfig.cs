namespace Logbook.Server.Infrastructure.Configuration
{
    public interface IAzureConfig
    {
        string EmailQueueName { get; }
        string AzureStorageConnectionString { get; }
    }
}