using System;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class RevokeRefreshTokenHandler : ICommandHandler<RevokeRefreshToken>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        
        public async Task HandleAsync(RevokeRefreshToken command)
        {
            var token = await _refreshTokenRepository.GetAsync(command.RefreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            token.Revoke(DateTime.UtcNow);
            await _refreshTokenRepository.UpdateAsync(token);
        }
    }
}