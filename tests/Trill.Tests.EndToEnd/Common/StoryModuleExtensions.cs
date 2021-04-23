using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Trill.Modules.Stories.Application.DTO;

namespace Trill.Tests.EndToEnd.Common
{
    internal static class StoryModuleExtensions
    {
        private const string Module = "stories-module";

        public static async Task<long> SendStoryAsync(this HttpClient client, Guid userId)
        {
            throw new NotImplementedException();
        }

        public static async Task<StoryDetailsDto> GetStoryAsync(this HttpClient client, long storyId)
        {
            var response = await client.GetAsync($"{Module}/stories/{storyId}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            return await response.ReadAsync<StoryDetailsDto>();
        }
    }
}