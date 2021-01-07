using System;
using Trill.Shared.Kernel.BuildingBlocks;

namespace Trill.Modules.Users.Core.Domain.Entities
{
    internal class Follower : AggregateRoot
    {
        public AggregateId FollowerId { get; }
        public AggregateId FolloweeId { get; }
        public DateTime CreatedAt { get; }

        public Follower(AggregateId id, AggregateId followerId, AggregateId followeeId, DateTime createdAt) : base(id)
        {
            FollowerId = followerId;
            FolloweeId = followeeId;
            CreatedAt = createdAt;
        }
    }
}