using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;

namespace Trill.Modules.Stories.Core.Services
{
    internal interface IStoryRatingService
    {
        Task<StoryRating> RateAsync(Story story, User user, int rate);
    }
}