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
            public static readonly string MicrosoftRequiredScope = "wl.emails";
            public static readonly string FacebookRequiredScope = "email";
        }
    }
}