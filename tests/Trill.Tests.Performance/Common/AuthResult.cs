using System;

namespace Trill.Tests.Performance.Common
{
    public class AuthResult
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}