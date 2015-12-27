using System;

namespace Logbook.Shared.Models
{
    public class AuthenticationToken
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}