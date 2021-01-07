using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Modules.Users.Core.Services;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class UseRefreshTokenHandler : ICommandHandler<UseRefreshToken>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ITokenStorage _storage;

        public UseRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository,
            IJwtProvider jwtProvider, ITokenStorage storage)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _storage = storage;
        }
        
        public async Task HandleAsync(UseRefreshToken command)
        {
            var token = await _refreshTokenRepository.GetAsync(command.RefreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            if (token.Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            var user = await _userRepository.GetAsync(token.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(token.UserId);
            }

            var claims = user.Permissions.Any()
                ? new Dictionary<string, IEnumerable<string>>
                {
                    ["permissions"] = user.Permissions
                }
                : null;
            var auth = _jwtProvider.Create(token.UserId, user.Name, user.Role, claims: claims);
            auth.RefreshToken = command.RefreshToken;
            _storage.Set(command.Id, auth);
        }
    }
}