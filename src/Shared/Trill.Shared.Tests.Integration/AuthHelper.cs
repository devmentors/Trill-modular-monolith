using System.Collections.Generic;
using Trill.Shared.Infrastructure.Auth;
using Trill.Shared.Infrastructure.Services;

namespace Trill.Shared.Tests.Integration
{
    public static class AuthHelper
    {
        private static readonly AuthManager AuthManager;

        static AuthHelper()
        {
            var options = OptionsHelper.GetOptions<AuthOptions>("auth");
            AuthManager = new AuthManager(options, new UtcClock());
        }

        public static string GenerateJwt(string userId, string role = null, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null)
            => AuthManager.CreateToken(userId, role, audience, claims).AccessToken;
    }
}
