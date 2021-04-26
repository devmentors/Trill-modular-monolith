using Trill.Modules.Stories.Core.Entities;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Events
{
    internal class StoryCreated : IDomainEvent
    {
        public Story Story { get; }

        public StoryCreated(Story story)
        {
            Story = story;
        }
    }
}