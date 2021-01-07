using System;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Saga.Commands.External
{
    [Message("ads")]
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