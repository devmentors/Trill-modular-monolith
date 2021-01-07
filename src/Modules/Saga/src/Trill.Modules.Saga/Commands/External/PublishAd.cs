using System;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Saga.Commands.External
{
    [Message("ads")]
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