using System;
using Shouldly;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.ValueObjects;
using Xunit;

namespace Trill.Modules.Stories.Tests.Unit.Entities
{
    public class StoryTests
    {
        [Fact]
        public void story_should_be_created_given_valid_data()
        {
            // Arrange
            var id = 1;
            var title = "Test";

            // Act
            var story = Create(id, title);

            // Assert
            story.Id.ShouldBe(id);
            story.Title.ShouldBe(title);
        }

        private static Story Create(long id = 1, string title = "Test")
        {
            var author = Author.Create(Guid.NewGuid(), "Test");
            var text = "Lorem ipsum";
            var tags = new[] {"tag1", "tag2"};
            var createdAt = DateTime.UtcNow;

            return Story.Create(id, author, title, text, tags, createdAt);
        }
    }
}