using Logbook.Server.Infrastructure.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Logbook.Server.Infrastructure
{
    public static class AppInsights
    {
        static AppInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = Config.Azure.ApplicationInsightsKey;
            Client = new TelemetryClient();
        }

        public static TelemetryClient Client { get; }
    }
}