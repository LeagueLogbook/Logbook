using System;

namespace Logbook.Shared.Models
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}