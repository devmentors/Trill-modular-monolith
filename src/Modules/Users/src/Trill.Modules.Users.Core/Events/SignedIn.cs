using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Users.Core.Events
{
    internal class SignedIn : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }

        public SignedIn(Guid userId)
        {
            UserId = userId;
        }
    }
}