using System.Collections.Generic;

namespace Trill.Shared.Abstractions.Auth
{
    public interface IAuthManager
    {
        JsonWebToken CreateToken(string userId, string role = null, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);
    }
}