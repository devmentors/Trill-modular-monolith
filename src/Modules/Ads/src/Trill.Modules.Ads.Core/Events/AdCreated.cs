using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Ads.Core.Events
{
    internal class AdCreated : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }

        public AdCreated(Guid adId)
        {
            AdId = adId;
        }
    }
}