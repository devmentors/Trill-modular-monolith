using System;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Entities
{
    internal class User : AggregateRoot<UserId>
    {
        public string Name { get; }
        public DateTime CreatedAt { get; }
        public int Rating { get; private set; }
        public bool Locked { get; private set; }

        public User(UserId id, string name, DateTime createdAt, int rating = 0, bool locked = false, int version = 0)
            : base(id, version)
        {
            Name = name;
            CreatedAt = createdAt;
            Rating = rating;
            Locked = locked;
        }

        public void Lock() => Locked = true;

        public void Unlock() => Locked = false;

        public void AddRating(int rating)
        {
            Rating += rating;
        }
    }
}