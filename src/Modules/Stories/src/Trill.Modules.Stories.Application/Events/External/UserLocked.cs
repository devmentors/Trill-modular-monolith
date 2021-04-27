using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Stories.Application.Events.External
{
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