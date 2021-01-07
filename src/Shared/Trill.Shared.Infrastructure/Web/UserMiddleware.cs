using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Trill.Shared.Infrastructure.Web
{
    internal class UserMiddleware : IMiddleware
    {
        private static readonly ISet<string> ValidMethods = new HashSet<string>
        {
            "POST", "PUT", "PATCH"
        };
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            if (!ValidMethods.Contains(request.Method))
            {
                await next(context);
                return;
            }

            if (!request.Headers.ContainsKey("authorization"))
            {
                await next(context);
                return;
            }

            var path = context.Request.Path.Value;
            if (path.Contains("sign-in") || path.Contains("sign-up"))
            {
                await next(context);
                return;
            }

            var authenticateResult = await context.AuthenticateAsync();
            if (!authenticateResult.Succeeded)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var content = string.Empty;
            if (request.Body is null)
            {
                await next(context);
                return;
            }

            context.User = authenticateResult.Principal;
            using (var reader = new StreamReader(request.Body))
            {
                content = await reader.ReadToEndAsync();
            }

            var payload = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
            payload["UserId"] = context.User.Identity.Name;
            var json = JsonSerializer.Serialize(payload);
            await using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            context.Request.Body = memoryStream;
            context.Request.ContentLength = json.Length;
            await next(context);
        }
    }
}