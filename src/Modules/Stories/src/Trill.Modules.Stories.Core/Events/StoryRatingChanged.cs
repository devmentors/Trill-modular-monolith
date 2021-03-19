using Trill.Modules.Stories.Core.Entities;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Events
{
    internal class StoryRatingChanged : IDomainEvent
    {
        public StoryRating Rating { get; }
        public int TotalRate { get; } 

        public StoryRatingChanged(StoryRating rating, int totalRate)
        {
            Rating = rating;
            TotalRate = totalRate;
        }
    }
}