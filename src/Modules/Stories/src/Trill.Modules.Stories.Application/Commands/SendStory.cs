using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Stories.Application.Commands
{
    internal class SendStory : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public long StoryId { get; }
        public Guid UserId { get; }
        public string Title { get; }
        public string Text { get; }
        public IEnumerable<string> Tags { get; }
        public DateTime? VisibleFrom { get; }
        public DateTime? VisibleTo { get; }
        public bool Highlighted { get; }

        public SendStory(long storyId, Guid userId, string title, string text, IEnumerable<string> tags,
            DateTime? visibleFrom = null, DateTime? visibleTo = null, bool highlighted = false)
        {
            StoryId = storyId;
            UserId = userId;
            Title = title;
            Text = text;
            Tags = tags ?? Enumerable.Empty<string>();
            VisibleFrom = visibleFrom;
            VisibleTo = visibleTo;
            Highlighted = highlighted;
        }
    }
}