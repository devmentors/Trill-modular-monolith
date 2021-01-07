using System;
using Trill.Modules.Stories.Core.Entities;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Documents
{
    internal class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Locked { get; set; }

        public UserDocument()
        {
        }
        
        public UserDocument(User user)
        {
            Id = user.Id;
            Name = user.Name;
            CreatedAt = user.CreatedAt;
            Locked = user.Locked;
        }

        public User ToEntity() => new User(Id, Name, CreatedAt, Locked);
    }
}