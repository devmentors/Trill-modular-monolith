using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidAggregateIdException : DomainException
    {
        public InvalidAggregateIdException() : base("Invalid aggregate id.")
        {
        }
    }
}