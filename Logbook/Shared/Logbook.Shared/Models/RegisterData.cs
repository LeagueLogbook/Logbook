namespace Logbook.Shared.Models
{
    public class RegisterData
    {
        public string EmailAddress { get; set; }
        public byte[] PasswordSHA256Hash { get; set; }
        public string PreferredLanguage { get; set; }
    }

    public class LoginData
    {
        public string EmailAddress { get; set; }
        public byte[] PasswordSHA256Hash { get; set; }
    }

    public class LiveLoginData
    {
        public string Code { get; set; }
        public string RedirectUrl { get; set; }
    }
}