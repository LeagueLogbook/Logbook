namespace Logbook.Shared.Entities.Authentication
{
    public class LiveAuthenticationKind : AuthenticationKindBase
    {
        public LiveAuthenticationKind()
        {
            this.Kind = AuthenticationKind.Live;
        }

        /// <summary>
        /// Gets or sets the Live user identifier.
        /// </summary>
        public string LiveUserId { get; set; }
    }
}