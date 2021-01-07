using System;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Users.Core.Mongo.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<RefreshTokenDocument, Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<RefreshTokenDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> GetAsync(string token)
        {
            var refreshToken = await _repository.GetAsync(x => x.Token == token);
            return refreshToken?.ToEntity();
        }

        public Task AddAsync(RefreshToken refreshToken)
            => _repository.AddAsync(new RefreshTokenDocument(refreshToken));

        public Task UpdateAsync(RefreshToken refreshToken)
            => _repository.UpdateAsync(new RefreshTokenDocument(refreshToken));
    }
}