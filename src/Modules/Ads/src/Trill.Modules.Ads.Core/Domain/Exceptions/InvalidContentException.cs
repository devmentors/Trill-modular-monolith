using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class InvalidContentException : DomainException
    {
        public InvalidContentException() : base("Invalid content.")
        {
        }
    }
}