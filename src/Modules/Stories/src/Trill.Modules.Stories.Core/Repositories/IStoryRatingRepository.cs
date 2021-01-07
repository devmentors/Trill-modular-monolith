using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;

namespace Trill.Modules.Stories.Core.Repositories
{
    internal interface IStoryRatingRepository
    {
        Task<int> GetTotalRatingAsync(StoryId storyId);
        Task SetAsync(StoryRating rating);
    }
}