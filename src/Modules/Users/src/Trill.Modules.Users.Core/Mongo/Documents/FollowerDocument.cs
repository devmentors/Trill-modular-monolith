using System;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Users.Core.Mongo.Documents
{
    internal class FollowerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid FollowerId { get; set; }
        public Guid FolloweeId { get;  set; }
        public DateTime CreatedAt { get; set; }

        public FollowerDocument()
        {
        }

        public FollowerDocument(Follower follower)
        {
            Id = follower.Id;
            FollowerId = follower.FollowerId;
            FolloweeId = follower.FolloweeId;
            CreatedAt = follower.CreatedAt;
        }

        public Follower ToEntity() => new Follower(Id, FollowerId, FolloweeId, CreatedAt);
    }
}