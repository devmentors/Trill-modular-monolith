using System;
using Shouldly;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Exceptions;
using Trill.Modules.Stories.Core.ValueObjects;
using Xunit;

namespace Trill.Modules.Stories.Tests.Unit.Entities
{
    public class StoryTests
    {
        [Fact]
        public void story_should_be_created_given_valid_data()
        {
            var id = 1;
            var author = Author.Create(Guid.NewGuid(), "Test");
            var title = "Test";
            var text = new StoryText("Lorem ipsum");
            var tags = new[] {"tag1", "tag2"};
            var createdAt = DateTime.UtcNow;
            
            var story = new Story(id, author, title, text, tags, createdAt);

            story.Id.ShouldBe(id);
            story.Author.ShouldBe(author);
            story.Title.ShouldBe(title);
            story.Text.ShouldBe(text);
            story.Tags.ShouldBe(tags);
            story.CreatedAt.ShouldBe(createdAt);
        }

        [Fact]
        public void story_creation_should_fail_given_invalid_title()
        {
            var title = string.Empty;
            
            var exception = Record.Exception(() => new Story(default, default, title, default, default, default));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStoryTitleException>();
        }
    }
}