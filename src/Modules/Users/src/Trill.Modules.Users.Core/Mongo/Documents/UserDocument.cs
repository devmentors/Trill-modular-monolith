using System;
using System.Collections.Generic;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Users.Core.Mongo.Documents
{
    internal class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public decimal Funds { get; set; }
        public bool Locked { get; set; }

        public UserDocument()
        {
        }

        public UserDocument(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            Role = user.Role;
            Password = user.Password;
            CreatedAt = user.CreatedAt;
            Permissions = user.Permissions;
            Funds = user.Funds;
            Locked = user.Locked;
        }

        public User ToEntity() => new User(Id, Email, Name, Password, Role, CreatedAt, Permissions, Funds, Locked);
    }
}