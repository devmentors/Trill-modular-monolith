using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Ads.Core.Commands
{
    internal class PayAd : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }

        public PayAd(Guid adId)
        {
            AdId = adId;
        }
    }
}