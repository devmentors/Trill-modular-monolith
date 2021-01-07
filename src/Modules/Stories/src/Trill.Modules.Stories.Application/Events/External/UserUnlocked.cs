using System;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Events.External
{
    [Message("users")]
    internal class UserUnlocked : IEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }

        public UserUnlocked(Guid userId)
        {
            UserId = userId;
        }
    }
}