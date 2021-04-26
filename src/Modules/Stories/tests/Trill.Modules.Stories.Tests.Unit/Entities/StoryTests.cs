using System;
using System.Linq;
using Shouldly;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Events;
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
            // Arrange
            var id = 1;
            var title = "Test";

            // Act
            var story = Create(id, title);

            // Assert
            story.Id.ShouldBe(id);
            story.Title.ShouldBe(title);
        }
        
        [Fact]
        public void new_story_should_contain_story_created_domain_event()
        {
            // Act
            var story = Create();
    
            // Assert
            story.Events.ShouldNotBeEmpty();
            var domainEvent = story.Events.Single();
            var storyCreatedEvent = domainEvent as StoryCreated;
            storyCreatedEvent.ShouldNotBeNull();
            storyCreatedEvent.Story.ShouldBe(story);
        }
        
        [Fact]
        public void story_creation_should_fail_given_invalid_title()
        {
            // Act
            var exception = Record.Exception(() => Create(title: string.Empty));

            // Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStoryTitleException>();
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