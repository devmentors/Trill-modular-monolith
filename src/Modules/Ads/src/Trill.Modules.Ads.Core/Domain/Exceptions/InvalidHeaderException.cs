using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class InvalidHeaderException : DomainException
    {
        public InvalidHeaderException() : base("Invalid title.")
        {
        }
    }
}