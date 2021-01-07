using System;
using System.Threading.Tasks;
using Trill.Web.Core.Services;
using Trill.Web.Core.Shared.DTO;
using Trill.Web.Core.Users.DTO;
using Trill.Web.Core.Users.Requests;

namespace Trill.Web.Core.Users
{
    public interface IUsersService
    {
        Task<ApiResponse<UserDetailsDto>> GetAsync(Guid userId);
        Task<ApiResponse> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<AuthDto>> LoginAsync(Login request);
        Task<ApiResponse<PagedDto<UserDto>>> BrowseUsersAsync(Guid? userId = null);
        Task<ApiResponse> FollowAsync(Guid followerId, Guid followeeId);
        Task<ApiResponse> UnfollowAsync(Guid followerId, Guid followeeId);
    }
}