using System;
using Trill.Modules.Users.Core.DTO;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Users.Core.Queries
{
    internal class BrowseUsers : PagedQueryBase, IQuery<Paged<UserDto>>
    {
        public Guid? UserId { get; set; }
    }
}