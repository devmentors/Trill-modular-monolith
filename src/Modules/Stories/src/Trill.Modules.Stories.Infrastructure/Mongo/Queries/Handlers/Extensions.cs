using System;
using System.Collections.Generic;
using System.Linq;
using Trill.Modules.Stories.Application.DTO;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Queries.Handlers
{
    internal static class Extensions
    {
        public static StoryDto ToDto(this StoryDocument story, IList<StoryRatingDocument> rates)
            => story.Map<StoryDto>(rates);

        public static StoryDetailsDto ToDetailsDto(this StoryDocument story, IList<StoryRatingDocument> rates,
            Guid? userId = null)
        {
            var dto = story.Map<StoryDetailsDto>(rates);
            dto.Text = story.Text;
            dto.UserRate = userId.HasValue
                ? rates.SingleOrDefault(x => x.StoryId == story.Id && x.UserId == userId)?.Rate ?? 0
                : 0;

            return dto;
        }

        private static T Map<T>(this StoryDocument story, IList<StoryRatingDocument> rates)
            where T : StoryDto, new()
            => new T
            {
                Id = story.Id,
                Author = new AuthorDto
                {
                    Id = story.Author.Id,
                    Name = story.Author.Name
                },
                Title = story.Title,
                Tags = story.Tags,
                Visibility = new VisibilityDto
                {
                    From = GetDate(story.From),
                    To = GetDate(story.To),
                    Highlighted = story.Highlighted
                },
                CreatedAt = GetDate(story.CreatedAt),
                TotalRate = rates.Sum(x => x.Rate),
            };

        private static DateTime GetDate(long timestamp) => DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
    }
}