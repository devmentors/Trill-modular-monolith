using System;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;
using Trill.Shared.Kernel.BuildingBlocks;

namespace Trill.Modules.Users.Core.Mongo.Repositories
{
    internal class FollowerRepository : IFollowerRepository
    {
        private readonly IMongoRepository<FollowerDocument, Guid> _repository;

        public FollowerRepository(IMongoRepository<FollowerDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Follower> GetAsync(AggregateId followerId, AggregateId followeeId)
        {
            var follower = await _repository.GetAsync(x => x.FollowerId == followerId && x.FolloweeId == followeeId);
            return follower?.ToEntity();
        }

        public Task AddAsync(Follower follower)
            => _repository.AddAsync(new FollowerDocument(follower));

        public Task DeleteAsync(AggregateId id)
            => _repository.DeleteAsync(id);
    }
}