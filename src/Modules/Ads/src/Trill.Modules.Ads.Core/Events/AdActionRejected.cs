using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Ads.Core.Events
{
    internal class AdActionRejected : IActionRejected
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }
        public string Code { get; }
        public string Reason { get; }

        public AdActionRejected(Guid adId, string code, string reason)
        {
            AdId = adId;
            Code = code;
            Reason = reason;
        }
    }
}