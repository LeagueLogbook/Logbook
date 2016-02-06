namespace Logbook.Shared.Entities.Authentication
{
    public abstract class AuthenticationKindBase : AggregateRoot
    {
        public User User { get; set; }
    }
}