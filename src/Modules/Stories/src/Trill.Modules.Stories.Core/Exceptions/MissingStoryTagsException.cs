using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class MissingStoryTagsException : DomainException
    {
        public MissingStoryTagsException() : base("Story tags are missing.")
        {
        }
    }
}