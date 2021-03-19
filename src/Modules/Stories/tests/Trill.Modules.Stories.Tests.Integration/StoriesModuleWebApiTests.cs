using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Trill.Bootstrapper;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.DTO;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Abstractions;
using Trill.Shared.Tests.Integration;
using Xunit;

namespace Trill.Modules.Stories.Tests.Integration
{
    [Collection("stories-module")]
    public class StoriesModuleWebApiTests : WebApiTestBase
    {
        [Fact]
        public async Task get_home_should_return_module_name()
        {
            var response = await GetAsync("");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("Stories module");
        }

        [Fact]
        public async Task post_send_story_should_create_story_and_return_location_header()
        {
            var storyId = GetRandomId();
            var userId = Guid.NewGuid();
            await AddUserToDatabaseAsync(userId);
            // await _userRepository.AddAsync(new User(userId, "Test", DateTime.UtcNow));
            var id = Guid.NewGuid();
            var command = new SendStory(storyId, userId, $"Test story {id:N}", $"Lorem ipsum {id}",
                new[] {$"test-1-{id:N}", $"test-2-{id:N}"});
            var response = await PostAsync("stories", command);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var location = response.Headers.Location;
            location.ShouldNotBeNull();
        }

        [Fact]
        public async Task get_story_should_return_story_details_dto()
        {
            var storyId = GetRandomId();
            await AddStoryToDatabaseAsync(storyId);
            var dto = await GetAsync<StoryDetailsDto>($"stories/{storyId}");
            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(storyId);
        }

        private static long GetRandomId() => Random.Next(1, int.MaxValue);

        private async Task AddUserToDatabaseAsync(Guid id)
        {
            var document = new UserDocument
            {
                Id = id,
                Name = $"user-{id}",
                CreatedAt = DateTime.UtcNow
            };
            await Mongo.Database.GetCollection<UserDocument>("stories-module.users")
                .InsertOneAsync(document);
        }

        private async Task AddStoryToDatabaseAsync(long id)
        {
            var document = new StoryDocument
            {
                Id = id,
                Title = $"Test story {id}",
                Text = $"Lorem ipsum {id}",
                Author = new AuthorDocument
                {
                    Id = Guid.NewGuid(),
                    Name = "Test"
                },
                Tags = new[] {"tag1", "tag2"},
                CreatedAt = DateTime.UtcNow.ToUnixTimeMilliseconds(),
                From = DateTime.UtcNow.ToUnixTimeMilliseconds(),
                To = DateTime.UtcNow.AddDays(1).ToUnixTimeMilliseconds()
            };
            await Mongo.Database.GetCollection<StoryDocument>("stories-module.stories")
                .InsertOneAsync(document);
        }

        private static readonly Random Random = new Random();
        private IUserRepository _userRepository;

        public StoriesModuleWebApiTests(WebApplicationFactory<Program> factory, MongoFixture mongo)
            : base(factory, mongo)
        {
            SetPath("stories-module");
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            _userRepository = new TestUserRepository();
            // services.AddSingleton(_userRepository);
        }

        private class TestUserRepository : IUserRepository
        {
            private readonly List<User> _users = new List<User>();

            public Task<User> GetAsync(UserId id) => Task.FromResult(_users.SingleOrDefault(x => x.Id.Equals(id)));

            public Task AddAsync(User user)
            {
                _users.Add(user);
                return Task.CompletedTask;
            }

            public Task UpdateAsync(User user) => Task.CompletedTask;
        }
    }
}