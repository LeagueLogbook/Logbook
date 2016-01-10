namespace Logbook.Shared.Entities.Authentication
{
    public class FacebookAuthenticationKind : AuthenticationKindBase
    {
        public FacebookAuthenticationKind()
        {
            this.Kind = AuthenticationKind.Facebook;
        }

        public string FacebookUserId { get; set; }
    }
}