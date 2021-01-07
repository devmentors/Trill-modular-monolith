using System;
using Trill.Modules.Stories.Application.Services;
using Trill.Shared.Abstractions.Storage;

namespace Trill.Modules.Stories.Infrastructure.Services
{
    internal class StoryRequestStorage : IStoryRequestStorage
    {
        private readonly IRequestStorage _storage;

        public StoryRequestStorage(IRequestStorage storage)
        {
            _storage = storage;
        }

        public void SetStoryId(Guid commandId, long storyId) => _storage.Set(GetKey(commandId), storyId);

        public long GetStoryId(Guid commandId) => _storage.Get<long>(GetKey(commandId));

        private static string GetKey(Guid commandId) => $"stories:{commandId}";
    }
}