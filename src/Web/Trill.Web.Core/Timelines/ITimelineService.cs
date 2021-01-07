using System;
using System.Threading.Tasks;
using Trill.Web.Core.Services;
using Trill.Web.Core.Shared.DTO;

namespace Trill.Web.Core.Timelines
{
    public interface ITimelineService
    {
        Task<ApiResponse<PagedDto<StoryDto>>> GetAsync(Guid userId);
    }
}