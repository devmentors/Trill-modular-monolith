using System;
using Trill.Modules.Users.Core.DTO;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Users.Core.Queries
{
    internal class GetUser : IQuery<UserDetailsDto>
    {
        public Guid UserId { get; set; }
    }
}