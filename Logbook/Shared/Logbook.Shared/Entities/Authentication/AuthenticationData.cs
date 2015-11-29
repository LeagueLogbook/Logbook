using System.Security.Cryptography;

namespace Logbook.Shared.Entities.Authentication
{
    public class AuthenticationData : AggregateRoot
    {
        /// <summary>
        /// Creates the Id for the <see cref="AuthenticationData"/> class.
        /// </summary>
        /// <param name="forUserId">For user identifier.</param>
        public static string CreateId(string forUserId) => $"{forUserId}/AuthenticationData";

        /// <summary>
        /// Gets or sets for user identifier.
        /// </summary>
        public string ForUserId { get; set; }

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