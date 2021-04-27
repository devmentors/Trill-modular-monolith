using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Clients.Users;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Factories;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Core.ValueObjects;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Time;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class SendStoryHandler : ICommandHandler<SendStory>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextFactory _storyTextFactory;
        private readonly IClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;
        private readonly IStoryAuthorPolicy _storyAuthorPolicy;
        private readonly IUsersApiClient _usersApiClient;

        public SendStoryHandler(IStoryRepository storyRepository, IStoryTextFactory storyTextFactory,
            IClock clock, IIdGenerator idGenerator, IStoryRequestStorage storyRequestStorage,
            IStoryAuthorPolicy storyAuthorPolicy, IUsersApiClient usersApiClient)
        {
            _storyRepository = storyRepository;
            _storyTextFactory = storyTextFactory;
            _clock = clock;
            _idGenerator = idGenerator;
            _storyRequestStorage = storyRequestStorage;
            _storyAuthorPolicy = storyAuthorPolicy;
            _usersApiClient = usersApiClient;
        }

        public async Task HandleAsync(SendStory command)
        {
            var userDto = await _usersApiClient.GetAsync(command.UserId);
            if (userDto is null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            
            // _storyAuthorPolicy.CanCreate()
            
            var author = Author.Create(command.UserId, $"user-{command.UserId:N}"); // Non-existent user for now
            var text = _storyTextFactory.Create(command.Text);
            var now = _clock.Current();
            var visibility = command.VisibleFrom.HasValue && command.VisibleTo.HasValue
                ? new Visibility(command.VisibleFrom.Value, command.VisibleTo.Value, command.Highlighted)
                : Visibility.Default(now);
            var storyId = command.StoryId <= 0 ? _idGenerator.Generate() : command.StoryId;
            var story = Story.Create(storyId, author, command.Title, text, command.Tags, now, visibility);
            await _storyRepository.AddAsync(story);
            _storyRequestStorage.SetStoryId(command.Id, story.Id);
        }
    }
}