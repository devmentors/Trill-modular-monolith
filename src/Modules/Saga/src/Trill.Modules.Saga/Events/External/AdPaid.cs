using System;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Saga.Events.External
{
    [Message("ads")]
    internal class AdPaid : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }

        public AdPaid(Guid adId)
        {
            AdId = adId;
        }
    }
}