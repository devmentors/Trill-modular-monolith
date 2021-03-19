using System.Linq;
using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Core.Services;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Kernel;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class RateStoryHandler : ICommandHandler<RateStory>
    {
        private readonly IStoryRatingRepository _storyRatingRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoryRatingService _storyRatingService;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public RateStoryHandler(IStoryRatingRepository storyRatingRepository, IStoryRepository storyRepository,
            IUserRepository userRepository, IStoryRatingService storyRatingService,
            IDomainEventDispatcher domainEventDispatcher, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _storyRatingRepository = storyRatingRepository;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
            _storyRatingService = storyRatingService;
            _domainEventDispatcher = domainEventDispatcher;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(RateStory command)
        {
            var story = await _storyRepository.GetAsync(command.StoryId);
            if (story is null)
            {
                throw new StoryNotFoundException(command.StoryId);
            }
            
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var rating = await _storyRatingService.RateAsync(story, user, command.Rate);
            await _storyRatingRepository.SetAsync(rating);
            var domainEvents = rating.Events.ToArray();
            await _domainEventDispatcher.DispatchAsync(domainEvents);

            var integrationEvents = _eventMapper.Map(domainEvents).ToArray();
            await _messageBroker.PublishAsync(integrationEvents);
        }
    }
}