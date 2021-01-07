using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class InvalidHeaderException : DomainException
    {
        public InvalidHeaderException() : base("Invalid title.")
        {
        }
    }
}