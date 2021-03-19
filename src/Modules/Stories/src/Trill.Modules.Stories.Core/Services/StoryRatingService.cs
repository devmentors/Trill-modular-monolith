using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Exceptions;
using Trill.Modules.Stories.Core.Repositories;

namespace Trill.Modules.Stories.Core.Services
{
    internal class StoryRatingService : IStoryRatingService
    {
        private readonly IStoryRatingRepository _storyRatingRepository;

        public StoryRatingService(IStoryRatingRepository storyRatingRepository)
        {
            _storyRatingRepository = storyRatingRepository;
        }
        
        public async Task<StoryRating> RateAsync(Story story, User user, int rate)
        {
            if (user.Locked)
            {
                throw new UserLockedException(user.Id);
            }
            
            var totalRating = await _storyRatingRepository.GetTotalRatingAsync(story.Id);
            var rating = StoryRating.Create(story.Id, user.Id, rate, totalRating);

            return rating;
        }
    }
}