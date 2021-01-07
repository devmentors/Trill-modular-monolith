using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Events;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class RateStoryHandler : ICommandHandler<RateStory>
    {
        private readonly IStoryRatingRepository _storyRatingRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;

        public RateStoryHandler(IStoryRatingRepository storyRatingRepository, IStoryRepository storyRepository,
            IUserRepository userRepository, IMessageBroker messageBroker)
        {
            _storyRatingRepository = storyRatingRepository;
            _storyRepository = storyRepository;
            _userRepository = userRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(RateStory command)
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

            var story = await _storyRepository.GetAsync(command.StoryId);
            if (story is null)
            {
                throw new StoryNotFoundException(command.StoryId);
            }

            await _storyRatingRepository.SetAsync(new StoryRating(new StoryRatingId(command.StoryId, command.UserId),
                command.Rate));
            var totalRating = await _storyRatingRepository.GetTotalRatingAsync(story.Id);
            await _messageBroker.PublishAsync(new StoryRated(command.StoryId, command.UserId,
                command.Rate, totalRating));
        }
    }
}