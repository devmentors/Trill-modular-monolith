using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.DTO;

namespace Trill.Tests.EndToEnd.Common
{
    internal static class UserModuleExtensions
    {
        private const string Module = "users-module";
        
        public static async Task SignUpAsync(this HttpClient client, Guid userId)
        {
            var command = new SignUp(userId, $"user-{userId}@trill.io", $"user-{userId}", "secret");
            var response = await client.PostAsync($"{Module}/sign-up", command.GetPayload());
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }
        
        public static async Task<AuthDto> SignInAsync(this HttpClient client, Guid userId)
        {
            var command = new SignIn($"user-{userId}", "secret");
            var response = await client.PostAsync($"{Module}/sign-in", command.GetPayload());
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            return await response.ReadAsync<AuthDto>();
        }
    }
}