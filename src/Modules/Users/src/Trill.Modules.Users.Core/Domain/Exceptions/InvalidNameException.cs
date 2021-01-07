using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidNameException : DomainException
    {
        public InvalidNameException(string name) : base($"Invalid name: {name}.")
        {
        }
    }
}