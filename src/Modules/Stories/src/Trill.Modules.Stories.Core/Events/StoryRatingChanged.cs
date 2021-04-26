using Trill.Modules.Stories.Core.Entities;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Events
{
    // internal record StoryRatingChanged(StoryRating StoryRating, int TotalRate) : IDomainEvent;
    
    internal class StoryRatingChanged : IDomainEvent
    {
        public StoryRating StoryRating { get; }
        public int TotalRate { get; }
    
        public StoryRatingChanged(StoryRating storyRating, int totalRate)
        {
            StoryRating = storyRating;
            TotalRate = totalRate;
        }
    }
}