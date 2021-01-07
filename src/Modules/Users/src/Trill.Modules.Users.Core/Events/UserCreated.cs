using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Users.Core.Events
{
    internal class UserCreated : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Role { get; }
        
        public UserCreated(Guid userId, string name, string role)
        {
            UserId = userId;
            Name = name;
            Role = role;
        }
    }
}