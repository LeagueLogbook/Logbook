namespace Logbook.Shared.Models
{
    public class RegisterData
    {
        public string EmailAddress { get; set; }
        public byte[] PasswordMD5Hash { get; set; }
        public string PreferredLanguage { get; set; }
    }
}