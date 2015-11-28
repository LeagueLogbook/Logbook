using System;
using System.Collections.Generic;
using Logbook.Shared.Configuration;

namespace Logbook.Server.Infrastructure
{
    public static class Config
    {
        static Config()
        {
            EnableDefaultMetrics = new BoolSetting("Logbook/EnableDefaultMetrics", false);
            CompressResponses = new BoolSetting("Logbook/CompressResponses", true);
            EnableDebugRequestResponseLogging = new BoolSetting("Logbook/EnableDebugRequestResponseLogging", false);
            FormatResponses = new BoolSetting("Logbook/FormatResponses", false);
            RavenHttpServerPort = new IntSetting("Logbook/RavenHttpServerPort", 8000);
            RavenName = new StringSetting("Logbook/RavenName", "Logbook");
            EnableRavenHttpServer = new BoolSetting("Logbook/EnableRavenHttpServer", false);
            Addresses = new UriListSetting("Logbook/Addresses", new List<Uri> { new Uri("http://localhost") }, "|");
        }

        public static BoolSetting EnableDefaultMetrics { get; }
        public static BoolSetting CompressResponses { get; }
        public static BoolSetting EnableDebugRequestResponseLogging { get; }
        public static BoolSetting FormatResponses { get; }
        public static IntSetting RavenHttpServerPort { get; }
        public static StringSetting RavenName { get; }
        public static BoolSetting EnableRavenHttpServer { get; }
        public static UriListSetting Addresses { get; }
    }
}