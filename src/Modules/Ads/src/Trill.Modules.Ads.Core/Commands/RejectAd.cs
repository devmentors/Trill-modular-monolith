using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Ads.Core.Commands
{
    internal class RejectAd : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }
        public string Reason { get; }

        public RejectAd(Guid adId, string reason)
        {
            AdId = adId;
            Reason = reason;
        }
    }
}