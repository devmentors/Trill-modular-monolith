using Trill.Modules.Stories.Core.Entities;

namespace Trill.Modules.Stories.Core.Policies
{
    internal class StoryAuthorPolicy : IStoryAuthorPolicy
    {
        public bool CanCreate(User user) => !user.Locked && user.Rating >= -10;
    }
}