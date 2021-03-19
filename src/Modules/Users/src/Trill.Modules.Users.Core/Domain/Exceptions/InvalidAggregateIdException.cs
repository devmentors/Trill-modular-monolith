using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidAggregateIdException : DomainException
    {
        public InvalidAggregateIdException() : base("Invalid aggregate id.")
        {
        }
    }
}