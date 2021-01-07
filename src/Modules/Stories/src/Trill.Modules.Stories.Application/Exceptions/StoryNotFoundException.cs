using Trill.Shared.Abstractions;

namespace Trill.Modules.Stories.Application.Exceptions
{
    internal class StoryNotFoundException : AppException
    {
        public StoryNotFoundException(long storyId) : base($"Story with ID: '{storyId}' was not found.")
        {
        }
    }
}