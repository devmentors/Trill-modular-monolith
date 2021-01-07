using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Clients.Stories.Requests;

namespace Trill.Modules.Ads.Core.Clients.Stories
{
    internal interface IStoryApiClient
    {
        Task<long?> SendStoryAsync(SendStory command);
    }
}