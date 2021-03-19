using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidStoryTagsException : DomainException
    {
        public InvalidStoryTagsException() : base("Story tags are invalid.")
        {
        }
    }
}