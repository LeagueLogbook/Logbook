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
            public static readonly string[] MicrosoftRequiredScopes = { "wl.basic", "wl.emails" };
            public static readonly string[] FacebookRequiredScopes = { "email", "public_profile" };
            public static readonly string[] GoogleRequiredScopes = { "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/plus.login" };
        }
    }
}