using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Models;

namespace Trill.Modules.Analytics.Core.Services
{
    internal interface IStoriesService
    {
        Task AddAsync(Story story);
        Task SetTotalRateAsync(long storyId, int totalRate);
    }
}