using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;

namespace Trill.Modules.Stories.Core.Repositories
{
    internal interface IStoryRepository
    {
        Task<Story> GetAsync(StoryId id);
        Task AddAsync(Story story);
    }
}