using System;

namespace Trill.Web.Core.Users.Requests
{
    public class FollowUser
    {
        public Guid UserId { get; set; }
        public Guid FolloweeId { get; set; }
    }
}