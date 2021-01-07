using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidStoryTitleException : DomainException
    {
        public InvalidStoryTitleException() : base("Invalid story title.")
        {
        }
    }
}