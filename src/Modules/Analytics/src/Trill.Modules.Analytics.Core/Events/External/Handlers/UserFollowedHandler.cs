using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Services;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Analytics.Core.Events.External.Handlers
{
    internal sealed class UserFollowedHandler : IEventHandler<UserFollowed>
    {
        private readonly IUsersService _usersService;

        public UserFollowedHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        public async Task HandleAsync(UserFollowed @event)
        {
            await _usersService.IncrementFollowersCountAsync(@event.FollowerId, @event.FolloweeId);
        }
    }
}