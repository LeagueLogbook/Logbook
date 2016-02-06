namespace Logbook.Shared.ControllerModels
{
    public class LoginData
    {
        public string EmailAddress { get; set; }
        public string PasswordSHA256Hash { get; set; }
    }
}