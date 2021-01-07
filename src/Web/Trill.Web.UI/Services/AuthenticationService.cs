using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Trill.Web.Core;
using Trill.Web.Core.Storage;
using Trill.Web.Core.Users;
using Trill.Web.Core.Users.Requests;

namespace Trill.Web.UI.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersService _usersService;
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;

        public User User { get; private set; }

        public AuthenticationService(IUsersService usersService, ILocalStorageService localStorageService,
            NavigationManager navigationManager)
        {
            _usersService = usersService;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
        }

        public async Task InitializeAsync()
        {
            User = await _localStorageService.GetItemAsync<User>("user");
        }

        public async Task<bool?> LoginAsync(string username, string password)
        {
            var response = await _usersService.LoginAsync(new Login
            {
                Name = username,
                Password = password
            });

            if (response is null || response.HttpResponse.StatusCode == HttpStatusCode.BadGateway)
            {
                return null;
            }

            if (!response.Succeeded)
            {
                return false;
            }

            User = new User
            {
                Id = response.Value.UserId,
                Name = response.Value.Username,
                Role = response.Value.Role,
                AccessToken = response.Value.AccessToken,
                RefreshToken = response.Value.RefreshToken,
                Expires = response.Value.Expires
            };
            await _localStorageService.SetItemAsync("user", User);

            return true;
        }

        public async Task LogoutAsync()
        {
            User = null;
            await _localStorageService.RemoveItemAsync("user");
            _navigationManager.NavigateTo("login");
        }
    }
}