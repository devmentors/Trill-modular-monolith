using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() : base("Invalid password.")
        {
        }
    }
}