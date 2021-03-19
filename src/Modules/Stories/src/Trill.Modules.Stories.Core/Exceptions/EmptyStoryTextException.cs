using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class EmptyStoryTextException : DomainException
    {
        public EmptyStoryTextException() : base("Empty story text.")
        {
        }
    }
}