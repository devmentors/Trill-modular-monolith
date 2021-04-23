using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Time;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class RevokeRefreshTokenHandler : ICommandHandler<RevokeRefreshToken>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IClock _clock;

        public RevokeRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, IClock clock)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _clock = clock;
        }
        
        public async Task HandleAsync(RevokeRefreshToken command)
        {
            var token = await _refreshTokenRepository.GetAsync(command.RefreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            token.Revoke(_clock.Current());
            await _refreshTokenRepository.UpdateAsync(token);
        }
    }
}