using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Services;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Analytics.Core.Events.External.Handlers
{
    internal sealed class StoryRatedHandler : IEventHandler<StoryRated>
    {
        private readonly IStoriesService _storiesService;

        public StoryRatedHandler(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }
    
        public async Task HandleAsync(StoryRated @event)
        {
            await _storiesService.SetTotalRateAsync(@event.StoryId, @event.TotalRate);
        }
    }
}