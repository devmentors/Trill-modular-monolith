using System.Collections.Generic;
using System.Threading.Tasks;
using Trill.Web.Core.Analytics.DTO;
using Trill.Web.Core.Services;
using Trill.Web.Core.Shared.DTO;

namespace Trill.Web.Core.Analytics.Services
{
    internal class AnalyticsService : IAnalyticsService
    {
        private readonly IHttpClient _client;

        public AnalyticsService(IHttpClient client)
        {
            _client = client;
        }

        public Task<ApiResponse<IEnumerable<TagDto>>> GetTopTagsAsync()
            => _client.GetAsync<IEnumerable<TagDto>>("analytics-module/trending/tags");

        public Task<ApiResponse<IEnumerable<StoryDto>>> GetTopStoriesAsync()
            => _client.GetAsync<IEnumerable<StoryDto>>("analytics-module/trending/stories");

        public Task<ApiResponse<IEnumerable<UserDto>>> GetTopUsersAsync()
            => _client.GetAsync<IEnumerable<UserDto>>("analytics-module/trending/users");
    }
}