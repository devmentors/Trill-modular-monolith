using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Models;
using Trill.Modules.Analytics.Core.Services;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Analytics.Core.Events.External.Handlers
{
    internal sealed class StorySentHandler : IEventHandler<StorySent>
    {
        private readonly IStoriesService _storiesService;
        private readonly ITagsService _tagsService;
        private readonly IUsersService _usersService;

        public StorySentHandler(IStoriesService storiesService, ITagsService tagsService,
            IUsersService usersService)
        {
            _storiesService = storiesService;
            _tagsService = tagsService;
            _usersService = usersService;
        }

        public async Task HandleAsync(StorySent @event)
        {
            await _storiesService.AddAsync(new Story
            {
                Id = @event.StoryId,
                Title = @event.Title,
                Author = new Author
                {
                    Id = @event.Author.Id,
                    Name = @event.Author.Name
                },
                CreatedAt = @event.CreatedAt,
                Tags = @event.Tags
            });

            await _usersService.IncrementStoriesCountAsync(@event.Author.Id);

            foreach (var tag in @event.Tags)
            {
                var isNew = await _tagsService.TryAddAsync(new Tag
                {
                    Name = tag,
                    OccurenceCount = 1
                });
                if (isNew)
                {
                    continue;
                }

                await _tagsService.IncrementOccurrencesCountAsync(tag);
            }
        }
    }
}