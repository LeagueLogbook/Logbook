namespace Logbook.Shared.Entities.Authentication
{
    public class TwitterAuthenticationKind : AuthenticationKindBase
    {
        public virtual string TwitterUserId { get; set; }
    }
}