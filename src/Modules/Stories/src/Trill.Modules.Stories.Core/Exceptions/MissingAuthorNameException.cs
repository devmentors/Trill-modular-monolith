using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class MissingAuthorNameException : DomainException
    {
        public MissingAuthorNameException() : base("Author name is missing.")
        {
        }
    }
}