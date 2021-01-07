using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class SignUp : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public string Password { get; }
        public string Role { get; }
        public IEnumerable<string> Permissions { get; }

        public SignUp(Guid userId, string email, string name, string password, string role = "user",
            IEnumerable<string> permissions = null)
        {
            UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
            Email = email;
            Name = name;
            Password = password;
            Role = role;
            Permissions = permissions ?? Enumerable.Empty<string>();
        }
    }
}