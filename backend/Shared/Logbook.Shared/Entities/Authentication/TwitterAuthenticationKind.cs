namespace Logbook.Shared.Entities.Authentication
{
    public class TwitterAuthenticationKind : AuthenticationKindBase
    {
        public TwitterAuthenticationKind()
        {
            this.Kind = AuthenticationKind.Twitter;
        }

        public string TwitterUserId { get; set; }
    }
}