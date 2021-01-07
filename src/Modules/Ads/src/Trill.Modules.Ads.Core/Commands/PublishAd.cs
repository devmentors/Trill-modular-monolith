using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Ads.Core.Commands
{
    internal class PublishAd : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid AdId { get; }

        public PublishAd(Guid adId)
        {
            AdId = adId;
        }
    }
}