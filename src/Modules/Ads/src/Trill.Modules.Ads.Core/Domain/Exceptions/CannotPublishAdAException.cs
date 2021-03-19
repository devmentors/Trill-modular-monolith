using System;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class CannotPublishAdAException : DomainException
    {
        public Guid AdId { get; }

        public CannotPublishAdAException(Guid adId) : base($"Ad with ID: '{adId}' cannot be published.")
        {
            AdId = adId;
        }
    }
}