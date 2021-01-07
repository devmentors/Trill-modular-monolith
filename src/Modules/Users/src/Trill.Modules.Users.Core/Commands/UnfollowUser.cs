using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class UnfollowUser : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public Guid FolloweeId { get; }

        public UnfollowUser(Guid userId, Guid followeeId)
        {
            UserId = userId;
            FolloweeId = followeeId;
        }
    }
}