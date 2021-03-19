using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class TooShortStoryTextException : DomainException
    {
        public TooShortStoryTextException( string text) : base($"Too short story text: '{text}'.")
        {
        }
    }
}