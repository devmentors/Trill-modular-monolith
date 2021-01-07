using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Timeline.Core.Events.External.Handlers
{
    internal sealed class UserFollowedHandler : IEventHandler<UserFollowed>
    {
        private readonly IStorage _storage;
        private readonly ILogger<UserFollowedHandler> _logger;

        public UserFollowedHandler(IStorage storage, ILogger<UserFollowedHandler> logger)
        {
            _storage = storage;
            _logger = logger;
        }
        
        public async Task HandleAsync(UserFollowed @event)
        {
            await _storage.FollowAsync(@event.FollowerId, @event.FolloweeId);
            _logger.LogInformation($"User with ID: '{@event.FollowerId}' followed '{@event.FollowerId}'.");
        }
    }
}