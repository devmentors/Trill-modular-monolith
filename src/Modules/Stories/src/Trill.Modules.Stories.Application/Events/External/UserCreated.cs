using System;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Events.External
{
    [Message("users")]
    internal class UserCreated : IEvent
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public string Name { get; }

        public UserCreated(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        private class Contract : Contract<UserCreated>
        {
            public Contract()
            {
                RequireAll();
            }
        }
    }
}