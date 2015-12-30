namespace Logbook.Shared.Models
{
    public class RegisterData
    {
        public string EmailAddress { get; set; }
        public string PasswordSHA256Hash { get; set; }
        public string PreferredLanguage { get; set; }
    }

    public class LoginData
    {
        public string EmailAddress { get; set; }
        public string PasswordSHA256Hash { get; set; }
    }

    public class MicrosoftLoginData
    {
        public string Code { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class FacebookLoginData
    {
        public string Code { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class GoogleLoginData
    {
        public string Code { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class PasswordResetData
    {
        public string EmailAddress { get; set; }
    }
}