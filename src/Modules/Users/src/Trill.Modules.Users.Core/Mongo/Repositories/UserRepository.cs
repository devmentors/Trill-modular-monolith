using System;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;
using Trill.Shared.Kernel.BuildingBlocks;

namespace Trill.Modules.Users.Core.Mongo.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetAsync(AggregateId id)
        {
            var document = await _repository.GetAsync(id);
            return document?.ToEntity();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return default;
            }
            
            var document = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());
            return document?.ToEntity();
        }

        public async Task<User> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return default;
            }
            
            var document = await _repository.GetAsync(x => x.Name == name.ToLowerInvariant());
            return document?.ToEntity();
        }

        public Task AddAsync(User user) => _repository.AddAsync(new UserDocument(user));

        public Task UpdateAsync(User user) => _repository.UpdateAsync(new UserDocument(user));
    }
}