using System;
using Trill.Modules.Users.Core.DTO;
using Trill.Shared.Abstractions.Storage;

namespace Trill.Modules.Users.Core.Services
{
    internal class TokenStorage : ITokenStorage
    {
        private readonly IRequestStorage _storage;

        public TokenStorage(IRequestStorage storage)
        {
            _storage = storage;
        }

        public void Set(Guid commandId, AuthDto token) => _storage.Set(GetKey(commandId), token);

        public AuthDto Get(Guid commandId) => _storage.Get<AuthDto>(GetKey(commandId));
        
        private static string GetKey(Guid commandId) => $"users:tokens:{commandId}";
    }
}