using System;
using System.Collections.Generic;
using Trill.Modules.Users.Core.DTO;
using Trill.Shared.Abstractions.Auth;

namespace Trill.Modules.Users.Core.Services
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly IAuthManager _authManager;

        public JwtProvider(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        public AuthDto Create(Guid userId, string username, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null)
        {
            var jwt = _authManager.CreateToken(userId.ToString("N"), role, audience, claims);

            return new AuthDto
            {
                UserId = userId,
                Name = username,
                AccessToken = jwt.AccessToken,
                Role = jwt.Role,
                Expires = jwt.Expires
            };
        }
    }
}