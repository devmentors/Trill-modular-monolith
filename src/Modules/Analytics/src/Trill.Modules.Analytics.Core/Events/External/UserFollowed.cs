using System;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Analytics.Core.Events.External
{
    [Message("users")]
    internal class UserFollowed : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid FollowerId { get; }
        public Guid FolloweeId { get; }

        public UserFollowed(Guid followerId, Guid followeeId)
        {
            FollowerId = followerId;
            FolloweeId = followeeId;
        }
    }
}