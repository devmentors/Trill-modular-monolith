using System;
using System.Threading.Tasks;
using Trill.Modules.Users.Core.DTO;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Modules.Users.Core.Queries;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Users.Core.Handlers.Queries
{
    internal class GetUserHandler : IQueryHandler<GetUser, UserDetailsDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;

        public GetUserHandler(IMongoRepository<UserDocument, Guid> userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<UserDetailsDto> HandleAsync(GetUser query)
        {
            var user = await _userRepository.GetAsync(query.UserId);
            
            return user is null
                ? null
                : new UserDetailsDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    Locked = user.Locked,
                    Permissions = user.Permissions,
                    Funds = user.Funds
                };
        }
    }
}