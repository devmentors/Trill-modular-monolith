using System;

namespace Trill.Shared.Abstractions.Kernel
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }
    }
}