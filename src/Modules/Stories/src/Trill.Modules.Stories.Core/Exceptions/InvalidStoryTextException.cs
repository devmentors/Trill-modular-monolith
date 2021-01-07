using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidStoryTextException : DomainException
    {
        public InvalidStoryTextException() : base("Invalid story text.")
        {
        }
    }
}