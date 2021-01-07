using System;
using System.Collections.Generic;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.ValueObjects;
using Trill.Shared.Abstractions;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Documents
{
    internal class StoryDocument : IIdentifiable<long>
    {
        public long Id { get; set; }
        public AuthorDocument Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public long CreatedAt { get; set; }
        public long From { get; set; }
        public long To { get; set; }
        public bool Highlighted { get;set; }

        public StoryDocument()
        {
        }

        public StoryDocument(Story story)
        {
            Id = story.Id;
            Author = new AuthorDocument(story.Author);
            Title = story.Title;
            Text = story.Text;
            Tags = story.Tags;
            CreatedAt = story.CreatedAt.ToUnixTimeMilliseconds();
            From = story.Visibility.From.ToUnixTimeMilliseconds();
            To = story.Visibility.To.ToUnixTimeMilliseconds();
            Highlighted = story.Visibility.Highlighted;
        }

        public Story ToEntity()
            => new Story(Id, Author.ToValueObject(), Text, Title, Tags,
                GetDate(CreatedAt), new Visibility(GetDate(From), GetDate(To), Highlighted));

        private static DateTime GetDate(long timestamp) => DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
    }
}