using System;
using System.Threading.Tasks;
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

        public RateStoryHandler(IStoryRatingRepository storyRatingRepository, IStoryRepository storyRepository,
            IStoryRatingService storyRatingService)
        {
            _storyRatingRepository = storyRatingRepository;
            _storyRepository = storyRepository;
            _storyRatingService = storyRatingService;
        }

        public async Task HandleAsync(RateStory command)
        {
            var story = await _storyRepository.GetAsync(command.StoryId);
            if (story is null)
            {
                throw new StoryNotFoundException(command.StoryId);
            }
        
            var user = new User(command.UserId, $"user-{command.UserId:N}", DateTime.UtcNow); // Non-existent user for now
            var rating = await _storyRatingService.RateAsync(story, user, command.Rate);
            await _storyRatingRepository.SetAsync(rating);
        }
    }
}