using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Core.Entities
{
    internal class StoryRating
    {
        public StoryRatingId Id { get; }
        public Rate Rate { get; }

        public StoryRating(StoryRatingId id,  Rate rate)
        {
            Id = id;
            Rate = rate;
        }
    }
}