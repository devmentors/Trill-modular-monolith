using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Trill.Api;
using Trill.Shared.Tests.Integration;
using Shouldly;
using Trill.Modules.Users.Core.Commands;
using Xunit;

namespace Trill.Modules.Users.Tests.Integration
{
    [Collection("users-module")]
    public class UsersModuleWebApiTests : WebApiTestBase
    {
        [Fact]
        public async Task get_home_should_return_module_name()
        {
            var response = await GetAsync("");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("Users module");
        }
        
        [Fact]
        public async Task post_sign_up_should_create_user_and_return_location_header()
        {
            var id = Guid.NewGuid();
            var command = new SignUp(id, $"user-{id}@trill.io", $"user-{id}", "secret");
            var response = await PostAsync("sign-up", command);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var location = response.Headers.Location;
            location.ShouldNotBeNull();
        }
        
        public UsersModuleWebApiTests(WebApplicationFactory<Program> factory, MongoFixture mongo) : base(factory, mongo)
        {
            SetPath("users-module");
        }
    }
}