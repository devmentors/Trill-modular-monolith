using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Repositories
{
    internal class StoryMongoRepository : IStoryRepository
    {
        private readonly IMongoRepository<StoryDocument, long> _repository;

        public StoryMongoRepository(IMongoRepository<StoryDocument, long> repository)
            => _repository = repository;

        public async Task<Story> GetAsync(StoryId id)
        {
            var document = await _repository.GetAsync(r => r.Id == id);
            return document?.ToEntity();
        }

        public Task AddAsync(Story story) => _repository.AddAsync(new StoryDocument(story));
    }
}