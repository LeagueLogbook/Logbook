using System.Collections.Generic;
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
        /// Initializes a new instance of the <see cref="AuthenticationData" /> class.
        /// </summary>
        public AuthenticationData()
        {
            this.Authentications = new List<AuthenticationKindBase>();
        }

        /// <summary>
        /// Gets or sets for user identifier.
        /// </summary>
        public string ForUserId { get; set; }
        /// <summary>
        /// Gets or sets the authentications.
        /// </summary>
        public List<AuthenticationKindBase> Authentications { get; set; }
    }
}