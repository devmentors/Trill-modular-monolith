using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Events;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Core.ValueObjects;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class SendStoryHandler : ICommandHandler<SendStory>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextPolicy _storyTextPolicy;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;
        private readonly IMessageBroker _messageBroker;

        public SendStoryHandler(IUserRepository userRepository, IStoryRepository storyRepository,
            IStoryTextPolicy storyTextPolicy, IDateTimeProvider dateTimeProvider, IIdGenerator idGenerator,
            IStoryRequestStorage storyRequestStorage, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _storyRepository = storyRepository;
            _storyTextPolicy = storyTextPolicy;
            _dateTimeProvider = dateTimeProvider;
            _idGenerator = idGenerator;
            _storyRequestStorage = storyRequestStorage;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(SendStory command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            if (user.Locked)
            {
                throw new UserLockedException(command.UserId);
            }

            var author = Author.Create(user);
            var storyText = new StoryText(command.Text);
            _storyTextPolicy.Verify(storyText);
            var now = _dateTimeProvider.Get();
            var visibility = command.VisibleFrom.HasValue && command.VisibleTo.HasValue
                ? new Visibility(command.VisibleFrom.Value, command.VisibleTo.Value, command.Highlighted)
                : Visibility.Default(now);
            var storyId = command.StoryId == default ? _idGenerator.Generate() : command.StoryId;
            var story = new Story(storyId, author, command.Title, storyText, command.Tags, now, visibility);
            await _storyRepository.AddAsync(story);
            _storyRequestStorage.SetStoryId(command.Id, story.Id);
            await _messageBroker.PublishAsync(new StorySent(story.Id,
                new StorySent.AuthorModel(author.Id, author.Name), story.Title, story.Tags, story.CreatedAt,
                new StorySent.VisibilityModel(story.Visibility.From, story.Visibility.To,
                    story.Visibility.Highlighted)));
        }
    }
}