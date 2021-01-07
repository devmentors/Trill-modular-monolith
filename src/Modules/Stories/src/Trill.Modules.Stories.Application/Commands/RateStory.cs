using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Stories.Application.Commands
{
    internal class RateStory : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public long StoryId { get; }
        public Guid UserId { get; }
        public int Rate { get; }

        public RateStory(long storyId, Guid userId, int rate)
        {
            StoryId = storyId;
            UserId = userId;
            Rate = rate;
        }
    }
}