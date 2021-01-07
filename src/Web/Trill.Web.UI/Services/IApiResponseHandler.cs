using System.Threading.Tasks;
using Trill.Web.Core.Services;

namespace Trill.Web.UI.Services
{
    public interface IApiResponseHandler
    {
        Task<ApiResponse> HandleAsync(Task<ApiResponse> request);
        Task<T> HandleAsync<T>(Task<ApiResponse<T>> request);
    }
}