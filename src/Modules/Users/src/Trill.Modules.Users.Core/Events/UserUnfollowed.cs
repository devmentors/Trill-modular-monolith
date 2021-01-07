using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Users.Core.Events
{
    internal class UserUnfollowed : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid FollowerId { get; }
        public Guid FolloweeId { get; }

        public UserUnfollowed(Guid followerId, Guid followeeId)
        {
            FollowerId = followerId;
            FolloweeId = followeeId;
        }
    }
}