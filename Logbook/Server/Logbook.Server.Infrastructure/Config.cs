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
            MaxSecondsToWaitForDatabaseToLoad = new IntSetting("Logbook/MaxSecondsToWaitForDatabaseToLoad", 20);
            RavenName = new StringSetting("Logbook/RavenName", "Logbook");
            EnableRavenHttpServer = new BoolSetting("Logbook/EnableRavenHttpServer", false);
            IterationCountForPasswordHashing = new IntSetting("Logbook/IterationCountForPasswordHashing", 10000);
            Addresses = new UriListSetting("Logbook/Addresses", new List<Uri> { new Uri("http://localhost") }, "|");
            AuthenticationKeyPhrase = new StringSetting("Logbook/AuthenticationKeyPhrase", string.Empty);
            LoginIsValidForDuration = new TimeSpanSetting("Logbook/LoginIsValidForDuration", TimeSpan.FromHours(8));
            MicrosoftClientId = new StringSetting("Logbook/MicrosoftClientId", string.Empty);
            MicrosoftClientSecret = new StringSetting("Logbook/MicrosoftClientSecret", string.Empty);
        }

        public static BoolSetting EnableDefaultMetrics { get; }
        public static BoolSetting CompressResponses { get; }
        public static BoolSetting EnableDebugRequestResponseLogging { get; }
        public static BoolSetting FormatResponses { get; }
        public static IntSetting RavenHttpServerPort { get; }
        public static IntSetting MaxSecondsToWaitForDatabaseToLoad { get; }
        public static StringSetting RavenName { get; }
        public static BoolSetting EnableRavenHttpServer { get; }
        public static IntSetting IterationCountForPasswordHashing { get; }
        public static UriListSetting Addresses { get; }
        public static StringSetting AuthenticationKeyPhrase { get; }
        public static TimeSpanSetting LoginIsValidForDuration { get; }
        public static StringSetting MicrosoftClientId { get; }
        public static StringSetting MicrosoftClientSecret { get; }

    }
}