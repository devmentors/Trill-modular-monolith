using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidRateException : DomainException
    {
        public int Rate { get; }

        public InvalidRateException(int rate) : base($"Invalid rate: {rate}")
        {
            Rate = rate;
        }
    }
}