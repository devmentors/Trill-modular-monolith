using System;

namespace Trill.Web.Core.Shared.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? StoriesCount { get; set; }
        public int? FollowersCount { get; set; }
        public int? FollowingCount { get; set; }
        public bool? IsFollowing { get; set; }
    }
}