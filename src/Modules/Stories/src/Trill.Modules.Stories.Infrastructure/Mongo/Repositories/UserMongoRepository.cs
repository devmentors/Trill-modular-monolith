using System;
using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Repositories
{
    internal class UserMongoRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserMongoRepository(IMongoRepository<UserDocument, Guid> repository)
            => _repository = repository;

        public async Task<User> GetAsync(UserId id)
        {
            var document = await _repository.GetAsync(r => r.Id == id);
            return document?.ToEntity();
        }

        public Task AddAsync(User user)
            => _repository.AddAsync(new UserDocument(user));

        public Task UpdateAsync(User user)
            => _repository.UpdateAsync(new UserDocument(user));
    }
}