namespace Logbook.Shared.Entities.Authentication
{
    public class LogbookAuthenticationKind : AuthenticationKind
    {
        /// <summary>
        /// Gets or sets the salt used for hashing the password.
        /// </summary>
        public byte[] Salt { get; set; }
        /// <summary>
        /// Gets or sets the iteration count for the hash algorithm.
        /// </summary>
        public int IterationCount { get; set; }
        /// <summary>
        /// Gets or sets the resulting hash.
        /// </summary>
        public byte[] Hash { get; set; }
    }
}