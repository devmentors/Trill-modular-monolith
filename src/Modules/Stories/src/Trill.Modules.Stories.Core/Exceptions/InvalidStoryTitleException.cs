using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidStoryTitleException : DomainException
    {
        public InvalidStoryTitleException() : base("Invalid story title.")
        {
        }
    }
}