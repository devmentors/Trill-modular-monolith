using System;
using System.Threading.Tasks;
using Trill.Web.Core.Services;
using Trill.Web.Core.Shared.DTO;
using Trill.Web.Core.Stories.Requests;

namespace Trill.Web.Core.Stories
{
    public interface IStoriesService
    {
        Task<ApiResponse<PagedDto<StoryDto>>> BrowseAsync(string query, Guid? userId = null);
        Task<ApiResponse<StoryDetailsDto>> GetAsync(long storyId, Guid? userId = null);
        Task<ApiResponse> SendAsync(SendStoryRequest request);
        Task<ApiResponse> SendUsingBrokerAsync(SendStoryRequest request);
        Task<ApiResponse> RateAsync(long storyId, int rate);
        Task<ApiResponse> RateUsingBrokerAsync(long storyId, int rate);
    }
}