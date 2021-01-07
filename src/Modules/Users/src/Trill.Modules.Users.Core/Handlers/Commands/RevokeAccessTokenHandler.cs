using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Shared.Abstractions.Auth;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class RevokeAccessTokenHandler : ICommandHandler<RevokeAccessToken>
    {
        private readonly IAccessTokenService _accessTokenService;

        public RevokeAccessTokenHandler(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }
        
        public async Task HandleAsync(RevokeAccessToken command)
        {
            await _accessTokenService.DeactivateAsync(command.AccessToken);
        }
    }
}