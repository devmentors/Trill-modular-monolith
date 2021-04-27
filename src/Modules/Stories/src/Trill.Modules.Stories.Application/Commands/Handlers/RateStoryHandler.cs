using System;
using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Clients.Users;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Core.Services;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class RateStoryHandler : ICommandHandler<RateStory>
    {
        private readonly IStoryRatingRepository _storyRatingRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryRatingService _storyRatingService;
        private readonly IUserRepository _userRepository;

        public RateStoryHandler(IStoryRatingRepository storyRatingRepository, IStoryRepository storyRepository,
            IStoryRatingService storyRatingService, IUserRepository userRepository)
        {
            _storyRatingRepository = storyRatingRepository;
            _storyRepository = storyRepository;
            _storyRatingService = storyRatingService;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(RateStory command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            
            var story = await _storyRepository.GetAsync(command.StoryId);
            if (story is null)
            {
                throw new StoryNotFoundException(command.StoryId);
            }
        
            var rating = await _storyRatingService.RateAsync(story, user, command.Rate);
            await _storyRatingRepository.SetAsync(rating);
        }
    }
}