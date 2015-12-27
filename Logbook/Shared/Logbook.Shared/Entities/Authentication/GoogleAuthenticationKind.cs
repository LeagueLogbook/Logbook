namespace Logbook.Shared.Entities.Authentication
{
    public class GoogleAuthenticationKind : AuthenticationKindBase
    {
        public GoogleAuthenticationKind()
        {
            this.Kind = AuthenticationKind.Google;
        }

        public string GoogleUserId { get; set; }
    }
}