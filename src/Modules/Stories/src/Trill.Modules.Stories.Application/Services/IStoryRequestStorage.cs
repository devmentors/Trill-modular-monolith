using System;

namespace Trill.Modules.Stories.Application.Services
{
    internal interface IStoryRequestStorage
    {
        void SetStoryId(Guid commandId, long storyId);
        long GetStoryId(Guid commandId);
    }
}