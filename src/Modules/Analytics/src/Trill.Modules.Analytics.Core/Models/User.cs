using System;

namespace Trill.Modules.Analytics.Core.Models
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StoriesCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}