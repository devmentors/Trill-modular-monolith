using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidCredentialsException : DomainException
    {
        public string Name { get; }

        public InvalidCredentialsException(string name) : base("Invalid credentials.")
        {
            Name = name;
        }
    }
}