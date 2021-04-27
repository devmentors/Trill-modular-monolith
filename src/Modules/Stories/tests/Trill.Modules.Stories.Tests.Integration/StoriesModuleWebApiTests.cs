using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Trill.Bootstrapper;
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
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("Stories module");
        }
        
        public StoriesModuleWebApiTests(WebApplicationFactory<Program> factory,
            MongoFixture mongo, string environment = "test") : base(factory, mongo, environment)
        {
            SetPath("stories-module");
        }
    }
}