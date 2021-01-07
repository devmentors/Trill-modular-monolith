using System;
using Trill.Shared.Abstractions;

namespace Trill.Modules.Users.Core.Exceptions
{
    internal class FollowerAndFolloweeIdAreTheSameException : AppException
    {
        public Guid UserId { get; }

        public FollowerAndFolloweeIdAreTheSameException(Guid userId)
            : base($"Follower and followee are the same user with ID: '{userId}'.")
        {
            UserId = userId;
        }
    }
}