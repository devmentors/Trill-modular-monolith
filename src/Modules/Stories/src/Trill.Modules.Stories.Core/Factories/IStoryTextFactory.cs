using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Core.Factories
{
    internal interface IStoryTextFactory
    {
        StoryText Create(string text);
    }
}