using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Modules.Stories.Core.Exceptions;
using Trill.Modules.Stories.Core.ValueObjects;

namespace Trill.Modules.Stories.Core.Entities
{
    internal class Story
    {
        private ISet<string> _tags = new HashSet<string>();
        public StoryId Id { get; }
        public Author Author { get; }
        public string Title { get; }
        public StoryText Text { get; }
        public Visibility Visibility { get; }

        public IEnumerable<string> Tags
        {
            get => _tags;
            private set => _tags = new HashSet<string>(value);
        }

        public DateTime CreatedAt { get; }

        public Story(StoryId id, Author author, string title, StoryText text, IEnumerable<string> tags,
            DateTime createdAt, Visibility visibility = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidStoryTitleException();
            }

            SetTags(tags);

            Id = id;
            Author = author;
            Title = title;
            Text = text;
            CreatedAt = createdAt;
            Visibility = visibility;
        }

        public void SetTags(IEnumerable<string> tags)
        {
            if (tags is null || !tags.Any())
            {
                throw new MissingStoryTagsException();
            }

            if (tags.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidStoryTagsException();
            }

            Tags = tags.Select(x => x.ToLowerInvariant().Trim().Replace(" ", "-"));
        }
    }
}