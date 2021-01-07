using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Models;

namespace Trill.Modules.Analytics.Core.Services
{
    internal interface ITrendingService
    {
        Task<IEnumerable<Story>> GetTopStoriesAsync();
        Task<IEnumerable<Tag>> GetTopTagsAsync();
        Task<IEnumerable<User>> GetTopUsersAsync();
    }
}