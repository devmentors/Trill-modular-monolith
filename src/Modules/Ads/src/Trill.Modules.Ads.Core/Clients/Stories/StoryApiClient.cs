using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Clients.Stories.Requests;
using Trill.Shared.Abstractions.Modules;

namespace Trill.Modules.Ads.Core.Clients.Stories
{
    internal class StoryApiClient : IStoryApiClient
    {
        private const string Module = "stories-module";
        private readonly IModuleClient _moduleClient;

        public StoryApiClient(IModuleClient moduleClient)
        {
            _moduleClient = moduleClient;
        }

        public async Task<long?> SendStoryAsync(SendStory command)
        {
            var response = await _moduleClient.RequestAsync<SendStory.Response>($"{Module}/send-story", command);

            return response?.StoryId;
        }
    }
}