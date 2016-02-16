using System;
using System.Runtime.Remoting.Messaging;
using Logbook.Server.Infrastructure.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Logbook.Server.Infrastructure
{
    public static class AppInsights
    {
        private const string CallContextDataKey = "AppInsights.OperationId";

        static AppInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = Config.Azure.ApplicationInsightsKey;
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new AsyncAwareOperationIdTelemetryInitializer());
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new TimeStampTelemetryInitializer());
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new StatusCodeFromResultTelemetryInitializer());

            Client = new TelemetryClient();
        }

        public static TelemetryClient Client { get; }

        public static void GenerateAsyncAwareOperationId()
        {
            CallContext.LogicalSetData(CallContextDataKey, Guid.NewGuid().ToString("D"));
        }
        
        #region Internal
        private class AsyncAwareOperationIdTelemetryInitializer : ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
                telemetry.Context.Operation.Id = (string)CallContext.LogicalGetData(CallContextDataKey);
            }
        }

        private class TimeStampTelemetryInitializer : ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
                telemetry.Timestamp = DateTimeOffset.UtcNow;
            }
        }

        private class StatusCodeFromResultTelemetryInitializer : ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
                var requestTelemetry = telemetry as RequestTelemetry;

                if (requestTelemetry?.Success != null && 
                    string.IsNullOrWhiteSpace(requestTelemetry.ResponseCode))
                {
                    requestTelemetry.ResponseCode = requestTelemetry.Success.Value ? "200" : "500";
                }
            }
        }
        #endregion
    }
}