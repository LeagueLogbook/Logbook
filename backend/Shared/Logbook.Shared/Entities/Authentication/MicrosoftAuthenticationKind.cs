namespace Logbook.Shared.Entities.Authentication
{
    public class MicrosoftAuthenticationKind : AuthenticationKindBase
    {
        /// <summary>
        /// Gets or sets the microsoft user identifier.
        /// </summary>
        public string MicrosoftUserId { get; set; }
    }
}