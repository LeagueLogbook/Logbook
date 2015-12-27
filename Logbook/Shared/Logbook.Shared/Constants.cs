namespace Logbook.Shared
{
    public static class Constants
    {
        public static class HttpApi
        {
            public static readonly string ContentType = "application/json";
        }

        public static class Authentication
        {
            public static readonly string AuthorizationHeaderType = "Bearer";
            public static readonly string AuthorizationQueryPart = "token";
            public static readonly string JWTIssuer = "Logbook";
        }

        public static class MicrosoftLogin
        {
            public static readonly string RequiredScope = "wl.emails";
        }
    }
}