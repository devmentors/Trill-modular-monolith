using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Timeline.Core.Events.External.Handlers
{
    internal sealed class UserUnfollowedHandler : IEventHandler<UserUnfollowed>
    {
        private readonly IStorage _storage;
        private readonly ILogger<UserUnfollowedHandler> _logger;

        public UserUnfollowedHandler(IStorage storage, ILogger<UserUnfollowedHandler> logger)
        {
            _storage = storage;
            _logger = logger;
        }
        
        public async Task HandleAsync(UserUnfollowed @event)
        {
            await _storage.UnfollowAsync(@event.FollowerId, @event.FolloweeId);
            _logger.LogInformation($"User with ID: '{@event.FollowerId}' unfollowed '{@event.FollowerId}'.");
        }
    }
}