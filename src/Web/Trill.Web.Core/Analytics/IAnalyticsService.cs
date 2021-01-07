using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Web.Core.Analytics.DTO;
using Trill.Web.Core.Services;
using Trill.Web.Core.Shared.DTO;

namespace Trill.Web.Core.Analytics
{
    public interface IAnalyticsService
    {
        Task<ApiResponse<IEnumerable<TagDto>>> GetTopTagsAsync();
        Task<ApiResponse<IEnumerable<StoryDto>>> GetTopStoriesAsync();
        Task<ApiResponse<IEnumerable<UserDto>>> GetTopUsersAsync();
    }
}