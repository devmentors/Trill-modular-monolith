using System;
using System.Threading.Tasks;
using Trill.Web.Core.Services;
using Trill.Web.Core.Shared.DTO;

namespace Trill.Web.Core.Timelines.Services
{
    internal class TimelineService : ITimelineService
    {
        private readonly IHttpClient _client;

        public TimelineService(IHttpClient client)
        {
            _client = client;
        }

        public Task<ApiResponse<PagedDto<StoryDto>>> GetAsync(Guid userId)
            => _client.GetAsync<PagedDto<StoryDto>>($"timeline-module/timelines/{userId}");
    }
}