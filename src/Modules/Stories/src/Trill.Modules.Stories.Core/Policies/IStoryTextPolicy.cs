using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Core.Policies
{
    internal interface IStoryTextPolicy
    {
        void Verify(StoryText storyText);
    }
}