using System;
using System.Collections.Generic;
using Trill.Modules.Users.Core.DTO;
using Trill.Shared.Abstractions.Auth;

namespace Trill.Modules.Users.Core.Services
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly IJwtHandler _jwtHandler;

        public JwtProvider(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public AuthDto Create(Guid userId, string username, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null)
        {
            var jwt = _jwtHandler.CreateToken(userId.ToString("N"), role, audience, claims);

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