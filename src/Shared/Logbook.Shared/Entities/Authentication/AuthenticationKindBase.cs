namespace Logbook.Shared.Entities.Authentication
{
    public abstract class AuthenticationKindBase : AggregateRoot
    {
        public virtual User User { get; set; }
    }
}