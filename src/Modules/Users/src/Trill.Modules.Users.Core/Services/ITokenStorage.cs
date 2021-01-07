using System;
using Trill.Modules.Users.Core.DTO;

namespace Trill.Modules.Users.Core.Services
{
    internal interface ITokenStorage
    {
        void Set(Guid commandId, AuthDto token);
        AuthDto Get(Guid commandId);
    }
}