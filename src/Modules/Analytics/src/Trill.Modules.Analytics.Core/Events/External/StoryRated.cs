using System;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Analytics.Core.Events.External
{
    [Message("stories")]
    internal class StoryRated : IEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public long StoryId { get; }
        public Guid UserId { get; }
        public int Rate { get; }
        public int TotalRate { get; }

        public StoryRated(long storyId, Guid userId, int rate, int totalRate)
        {
            StoryId = storyId;
            UserId = userId;
            Rate = rate;
            TotalRate = totalRate;
        }
    }
}