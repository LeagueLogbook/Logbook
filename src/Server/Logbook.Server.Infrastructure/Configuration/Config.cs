namespace Logbook.Server.Infrastructure.Configuration
{
    public static class Config
    {
        public static IHttpConfig Http { get; set; }
        public static IAzureConfig Azure { get; set; }
        public static IEmailConfig Email { get; set; }
        public static IAppConfig App { get; set; }
        public static IEmailTemplateConfig EmailTemplate { get; set; }
        public static ISecurityConfig Security { get; set; }
        public static IRiotConfig Riot { get; set; }
        public static IDatabaseConfig Database { get; set; }
    }
}