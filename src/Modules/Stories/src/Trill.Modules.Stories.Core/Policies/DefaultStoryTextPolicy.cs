using Trill.Modules.Stories.Core.Exceptions;
using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Core.Policies
{
    internal class DefaultStoryTextPolicy : IStoryTextPolicy
    {
        public void Verify(StoryText storyText)
        {
            if (storyText is null)
            {
                throw new InvalidStoryTextException();
            }
            
            if (string.IsNullOrWhiteSpace(storyText.Value))
            {
                throw new EmptyStoryTextException();
            }

            if (storyText.Value.Length < 10)
            {
                throw new TooShortStoryTextException(storyText.Value);
            }

            if (storyText.Value.Length > 200)
            {
                throw new TooLongStoryTextException($"{storyText.Value.Substring(200)}...");
            }
        }
    }
}