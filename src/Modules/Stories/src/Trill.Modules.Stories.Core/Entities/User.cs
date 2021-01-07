using System;

namespace Trill.Modules.Stories.Core.Entities
{
    internal class User
    {
        public UserId Id { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }
        public bool Locked { get; private set; }

        public User(UserId id, string name, DateTime createdAt, bool locked = false)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
            Locked = locked;
        }

        public void Lock() => Locked = true;

        public void Unlock() => Locked = false;
    }
}