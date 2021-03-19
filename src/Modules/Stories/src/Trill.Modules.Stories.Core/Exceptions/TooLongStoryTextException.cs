using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class TooLongStoryTextException : DomainException
    {
        public TooLongStoryTextException( string text) : base($"Too long story text: '{text}'.")
        {
        }
    }
}