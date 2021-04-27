using System.Threading.Tasks;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Timeline.Core.Events.External.Handlers
{
    internal sealed class StoryRatedHandler : IEventHandler<StoryRated>
    {
        private readonly IStorage _storage;

        public StoryRatedHandler(IStorage storage)
        {
            _storage = storage;
        }
    
        public async Task HandleAsync(StoryRated @event)
        {
            await _storage.SetStoryTotalRatingAsync(@event.StoryId, @event.TotalRate);
        }
    }
}