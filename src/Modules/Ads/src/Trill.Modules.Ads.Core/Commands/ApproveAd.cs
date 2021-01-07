using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Ads.Core.Commands
{
    internal class ApproveAd : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }

        public ApproveAd(Guid adId)
        {
            AdId = adId;
        }

    }
}