using Trill.Modules.Stories.Core.Entities;

namespace Trill.Modules.Stories.Core.Policies
{
    internal interface IStoryAuthorPolicy
    {
        bool CanCreate(User user);
    }
}