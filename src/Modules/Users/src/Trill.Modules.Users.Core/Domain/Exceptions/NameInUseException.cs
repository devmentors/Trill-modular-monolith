using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class NameInUseException : DomainException
    {
        public string Name { get; }

        public NameInUseException(string name) : base($"Name {name} is already in use.")
        {
            Name = name;
        }
    }
}