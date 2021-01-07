using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.DTO;

namespace Trill.Tests.EndToEnd.Common
{
    internal static class StoryModuleExtensions
    {
        private const string Module = "stories-module";

        public static async Task<long> SendStoryAsync(this HttpClient client, Guid userId)
        {
            var id = Guid.NewGuid();
            var command = new SendStory(default, userId, $"Test story {id:N}", $"Lorem ipsum {id}",
                new[] {$"test-1-{id:N}", $"test-2-{id:N}"});
            var response = await client.PostAsync($"{Module}/stories", command.GetPayload());
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var location = response.Headers.Location;
            location.ShouldNotBeNull();
            var storyId = long.Parse(location.ToString().Split("/").Last());
            return storyId;
        }

        public static async Task<StoryDetailsDto> GetStoryAsync(this HttpClient client, long storyId)
        {
            var response = await client.GetAsync($"{Module}/stories/{storyId}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            return await response.ReadAsync<StoryDetailsDto>();
        }
    }
}