namespace Logbook.Shared.Entities.Authentication
{
    public class LogbookAuthenticationKind : AuthenticationKindBase
    {
        /// <summary>
        /// Gets or sets the salt used for hashing the password.
        /// </summary>
        public virtual byte[] Salt { get; set; }
        /// <summary>
        /// Gets or sets the iteration count for the hash algorithm.
        /// </summary>
        public virtual int IterationCount { get; set; }
        /// <summary>
        /// Gets or sets the resulting hash.
        /// </summary>
        public virtual byte[] Hash { get; set; }
    }
}