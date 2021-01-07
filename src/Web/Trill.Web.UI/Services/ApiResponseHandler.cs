using System.Net;
using System.Threading.Tasks;
using AntDesign;
using Trill.Web.Core.Services;

namespace Trill.Web.UI.Services
{
    internal class ApiResponseHandler : IApiResponseHandler
    {
        private const int ModalDurationSeconds = 1;
        private readonly MessageService _messageService;
        private readonly IAuthenticationService _authenticationService;

        public ApiResponseHandler(MessageService messageService, IAuthenticationService authenticationService)
        {
            _messageService = messageService;
            _authenticationService = authenticationService;
        }

        public async Task<ApiResponse> HandleAsync(Task<ApiResponse> request)
        {
            var response = await request;
            if (response.Succeeded)
            {
                return response;
            }

            await HandleErrorAsync(response);
            return default;
        }

        public async Task<T> HandleAsync<T>(Task<ApiResponse<T>> request)
        {
            var response = await request;
            if (response.Succeeded)
            {
                return response.Value;
            }

            await HandleErrorAsync(response);
            return default;
        }

        private async Task HandleErrorAsync(ApiResponse response)
        {
            if (response.HttpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _messageService.Error("Your session has expired - please sign in again.");
                await _authenticationService.LogoutAsync();
                return;
            }

            if (response.Error is {})
            {
                await _messageService.Error(response.Error.Reason, ModalDurationSeconds);
            }
        }
    }
}