using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Trill.Api;
using Trill.Shared.Tests.Integration;
using Trill.Tests.EndToEnd.Common;
using Xunit;

namespace Trill.Tests.EndToEnd
{
    public class StoryScenarioTests : WebApiTestBase
    {
        [Fact]
        public async Task given_new_user_account_story_should_be_sent()
        {
            var userId = Guid.NewGuid();
            await Client.SignUpAsync(userId);
            await WaitForEventsAsync();
            var auth = await Client.SignInAsync(userId);
            var storyId = await Client.SendStoryAsync(auth.UserId);
            var storyDto = await Client.GetStoryAsync(storyId);
            storyDto.ShouldNotBeNull();
            storyDto.Id.ShouldBe(storyId);
        }

        private static Task WaitForEventsAsync() => Task.Delay(2000);

        public StoryScenarioTests(WebApplicationFactory<Program> factory, MongoFixture mongo) : base(factory, mongo,
            "test-e2e")
        {
        }
    }
}