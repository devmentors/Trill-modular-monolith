using System;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Events.External
{
    [Message("users")]
    internal class UserLocked : IEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }

        public UserLocked(Guid userId)
        {
            UserId = userId;
        }
    }
}