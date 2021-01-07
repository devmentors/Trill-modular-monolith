using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class CannotPayAdException : DomainException
    {
        public Guid AdId { get; }

        public CannotPayAdException(Guid adId) : base($"Ad with ID: '{adId}' cannot be paid.")
        {
            AdId = adId;
        }
    }
}