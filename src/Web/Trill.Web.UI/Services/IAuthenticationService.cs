using System.Threading.Tasks;
using Trill.Web.Core;

namespace Trill.Web.UI.Services
{
    public interface IAuthenticationService
    {
        User User { get; }
        Task InitializeAsync();
        Task<bool?> LoginAsync(string username, string password);
        Task LogoutAsync();
    }
}