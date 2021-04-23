using Trill.Modules.Stories.Core.ValueObjects;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Entities
{
    internal class StoryRating : AggregateRoot<StoryRatingId>
    {
        public Rate Rate { get; }

        public StoryRating(StoryRatingId id, Rate rate, int version = 0) : base(id, version)
        {
            Rate = rate;
        }

        public static StoryRating Create(StoryId storyId, UserId userId, int rate, int totalRate)
        {
            var rating = new StoryRating(new StoryRatingId(storyId, userId), new Rate(rate));

            return rating;
        }
    }
}