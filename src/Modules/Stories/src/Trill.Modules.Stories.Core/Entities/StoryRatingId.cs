namespace Trill.Modules.Stories.Core.Entities
{
    internal class StoryRatingId
    {
        public StoryId StoryId { get; }
        public UserId UserId { get; }

        public StoryRatingId(StoryId storyId, UserId userId)
        {
            StoryId = storyId;
            UserId = userId;
        }
    }
}