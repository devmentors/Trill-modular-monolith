using System;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class AdNotFoundException : DomainException
    {
        public Guid AdId { get; }

        public AdNotFoundException(Guid adId) : base($"Ad with ID: '{adId}' was not found.")
        {
            AdId = adId;
        }
    }
}