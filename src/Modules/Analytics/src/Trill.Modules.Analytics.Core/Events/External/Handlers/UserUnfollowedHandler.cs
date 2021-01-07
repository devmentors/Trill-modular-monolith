using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Services;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Analytics.Core.Events.External.Handlers
{
    internal sealed class UserUnfollowedHandler : IEventHandler<UserUnfollowed>
    {
        private readonly IUsersService _usersService;

        public UserUnfollowedHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task HandleAsync(UserUnfollowed @event)
        {
            await _usersService.DecrementFollowersCountAsync(@event.FollowerId, @event.FolloweeId);
        }
    }
}