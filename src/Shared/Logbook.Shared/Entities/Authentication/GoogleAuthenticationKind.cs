namespace Logbook.Shared.Entities.Authentication
{
    public class GoogleAuthenticationKind : AuthenticationKindBase
    {
        public virtual string GoogleUserId { get; set; }
    }
}