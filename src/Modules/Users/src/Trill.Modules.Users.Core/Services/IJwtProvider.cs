using System;
using System.Collections.Generic;
using Trill.Modules.Users.Core.DTO;

namespace Trill.Modules.Users.Core.Services
{
    internal interface IJwtProvider
    {
        AuthDto Create(Guid userId, string username, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);
    }
}