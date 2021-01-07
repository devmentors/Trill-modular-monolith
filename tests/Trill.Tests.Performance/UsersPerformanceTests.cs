using System;
using System.Net.Http;
using NBomber.Plugins.Http.CSharp;
using Shouldly;
using Trill.Tests.Performance.Common;
using Xunit;
using Xunit.Abstractions;

namespace Trill.Tests.Performance
{
    public class UsersPerformanceTests : PerformanceTestsBase
    {
        [Fact]
        public void post_sign_up()
            => RunScenario("POST users-module/sign-up", () => Http
                    .CreateRequest("POST", $"{_url}/sign-up")
                    .WithBody(CreateRandomUserPayload())
                    .WithCheck(AssertOkResponse))
                .Assert(stats => { stats.RequestCount.ShouldBeGreaterThanOrEqualTo(1000); });

        #region Arrange

        private readonly string _url;

        public UsersPerformanceTests(ITestOutputHelper output) : base(output)
        {
            _url = $"{ApiUrl}/users-module";
        }

        private static StringContent CreateRandomUserPayload()
        {
            var id = Guid.NewGuid();
            var command = new
            {
                id,
                email = $"user-{id:N}@trill.io",
                name = $"user-{id:N}",
                password = "secret",
                role = "user"
            };
            return GetPayload(command);
        }

        #endregion
    }
}