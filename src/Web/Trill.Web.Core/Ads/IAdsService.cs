using System;
using System.Threading.Tasks;
using Trill.Web.Core.Ads.DTO;
using Trill.Web.Core.Ads.Requests;
using Trill.Web.Core.Services;

namespace Trill.Web.Core.Ads
{
    public interface IAdsService
    {
        Task<ApiResponse<PagedDto<AdDto>>> BrowseAsync(Guid? userId = null);
        Task<ApiResponse<AdDetailsDto>> GetAsync(Guid adId);
        Task<ApiResponse> CreateAsync(CreateAdRequest request);
        Task<ApiResponse> ApproveAsync(Guid adId);
        Task<ApiResponse> RejectAsync(Guid adId);
    }
}