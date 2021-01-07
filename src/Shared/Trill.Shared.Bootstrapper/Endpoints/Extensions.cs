using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Bootstrapper.Endpoints
{
    public static class Extensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(new CamelCaseNamingStrategy())
            }
        };
        
        private static readonly byte[] InvalidJsonRequestBytes = Encoding.UTF8.GetBytes("An invalid JSON was sent.");
        private const string EmptyJsonObject = "{}";
        private const string LocationHeader = "Location";
        private const string JsonContentType = "application/json";

        public static IEndpointRouteBuilder Get<TQuery, TResult>(this IEndpointRouteBuilder builder, string path,
            Action<IEndpointConventionBuilder> endpoint = null,
            Func<TQuery, HttpContext, Task> before = null,
            Func<TQuery, TResult, HttpContext, Task> after = null,
            bool auth = false, string roles = null,
            params string[] policies)
            where TQuery : class, IQuery<TResult>
        {
            var conventionBuilder = builder.MapGet(path, ctx => HandleQueryAsync(ctx, before, after));
            endpoint?.Invoke(conventionBuilder);
            ApplyAuthRolesAndPolicies(conventionBuilder, auth, roles, policies);

            return builder;
        }
        
        public static IEndpointRouteBuilder Post<T>(this IEndpointRouteBuilder builder, string path,
            Action<IEndpointConventionBuilder> endpoint = null,
            Func<T, HttpContext, Task> before = null,
            Func<T, HttpContext, Task> after = null,
            bool auth = false, string roles = null,
            params string[] policies)
            where T : class, ICommand
        {
            var conventionBuilder = builder.MapPost(path, ctx => HandleCommandAsync(ctx, before, after));
            endpoint?.Invoke(conventionBuilder);
            ApplyAuthRolesAndPolicies(conventionBuilder, auth, roles, policies);

            return builder;
        }
        
        public static IEndpointRouteBuilder Put<T>(this IEndpointRouteBuilder builder, string path,
            Action<IEndpointConventionBuilder> endpoint = null,
            Func<T, HttpContext, Task> before = null,
            Func<T, HttpContext, Task> after = null,
            bool auth = false, string roles = null,
            params string[] policies)
            where T : class, ICommand
        {
            var conventionBuilder = builder.MapPut(path, ctx => HandleCommandAsync(ctx, before, after));
            endpoint?.Invoke(conventionBuilder);
            ApplyAuthRolesAndPolicies(conventionBuilder, auth, roles, policies);

            return builder;
        }
        
        public static IEndpointRouteBuilder Delete<T>(this IEndpointRouteBuilder builder, string path,
            Action<IEndpointConventionBuilder> endpoint = null,
            Func<T, HttpContext, Task> before = null,
            Func<T, HttpContext, Task> after = null,
            bool auth = false, string roles = null,
            params string[] policies)
            where T : class, ICommand
        {
            var conventionBuilder = builder.MapDelete(path, ctx => HandleCommandAsync(ctx, before, after, false));
            endpoint?.Invoke(conventionBuilder);
            ApplyAuthRolesAndPolicies(conventionBuilder, auth, roles, policies);

            return builder;
        }

        private static async Task HandleCommandAsync<T>(HttpContext context, Func<T, HttpContext, Task> before = null,
            Func<T, HttpContext, Task> after = null, bool readFromBody = true) where T : class, ICommand
        {
            var command = readFromBody ? await context.ReadJsonAsync<T>() : context.ReadQuery<T>();
            if (before is {})
            {
                await before(command, context);
            }

            var dispatcher = context.RequestServices.GetRequiredService<ICommandDispatcher>();
            await dispatcher.SendAsync(command);
            if (after is null)
            {
                context.Response.StatusCode = 200;
                return;
            }

            await after(command, context);
        }

        private static async Task HandleQueryAsync<TQuery, TResult>(HttpContext context,
            Func<TQuery, HttpContext, Task> before = null,
            Func<TQuery, TResult, HttpContext, Task> after = null) where TQuery : class, IQuery<TResult>
        {
            var query = context.ReadQuery<TQuery>();
            if (before is {})
            {
                await before(query, context);
            }

            var dispatcher = context.RequestServices.GetRequiredService<IQueryDispatcher>();
            var result = await dispatcher.QueryAsync<TQuery, TResult>(query);
            if (after is null)
            {
                if (result is null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                await context.Response.WriteJsonAsync(result);
                return;
            }

            await after(query, result, context);
        }

        private static void ApplyAuthRolesAndPolicies(IEndpointConventionBuilder builder, bool auth, string roles,
            params string[] policies)
        {
            if (policies is {} && policies.Any())
            {
                builder.RequireAuthorization(policies);
                return;
            }

            var hasRoles = !string.IsNullOrWhiteSpace(roles);
            var authorize = new AuthorizeAttribute();
            if (hasRoles)
            {
                authorize.Roles = roles;
            }

            if (auth || hasRoles)
            {
                builder.RequireAuthorization(authorize);
            }
        }
        private static T ReadQuery<T>(this HttpContext context) where T : class
        {
            var request = context.Request;
            RouteValueDictionary values = null;
            if (HasRouteData(request))
            {
                values = request.HttpContext.GetRouteData().Values;
            }

            if (HasQueryString(request))
            {
                var queryString = HttpUtility.ParseQueryString(request.HttpContext.Request.QueryString.Value);
                values ??= new RouteValueDictionary();
                foreach (var key in queryString.AllKeys)
                {
                    values.TryAdd(key, queryString[key]);
                }
            }

            if (values is null)
            {
                return JsonConvert.DeserializeObject<T>(EmptyJsonObject, SerializerSettings);
            }

            var serialized = JsonConvert.SerializeObject(values.ToDictionary(k => k.Key, k => k.Value))
                ?.Replace("\\\"", "\"")
                .Replace("\"{", "{")
                .Replace("}\"", "}")
                .Replace("\"[", "[")
                .Replace("]\"", "]");

            return JsonConvert.DeserializeObject<T>(serialized, SerializerSettings);
        }
        
        private static bool HasQueryString(this HttpRequest request)
            => request.Query.Any();
        
        private static bool HasRouteData(this HttpRequest request)
            => request.HttpContext.GetRouteData().Values.Any();
        
        public static async Task<T> ReadJsonAsync<T>(this HttpContext httpContext)
        {
            if (httpContext.Request.Body is null)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.Body.WriteAsync(InvalidJsonRequestBytes, 0, InvalidJsonRequestBytes.Length);

                return default;
            }

            try
            {
                var request = httpContext.Request;
                var json = await new StreamReader(request.Body).ReadToEndAsync();
                var payload = JsonConvert.DeserializeObject<T>(json, SerializerSettings);
                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(payload, new ValidationContext(payload), results))
                {
                    return payload;
                }

                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteJsonAsync(results);

                return default;
            }
            catch
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.Body.WriteAsync(InvalidJsonRequestBytes, 0, InvalidJsonRequestBytes.Length);

                return default;
            }
        }
        
        public static async Task WriteJsonAsync<T>(this HttpResponse response, T value)
        {
            response.ContentType = JsonContentType;
            var json = JsonConvert.SerializeObject(value, SerializerSettings);
            await response.WriteAsync(json);
        }
        
        public static Task Ok(this HttpResponse response, object data = null)
        {
            response.StatusCode = 200;
            return data is null ? Task.CompletedTask : response.WriteJsonAsync(data);
        }

        public static Task Created(this HttpResponse response, string location = null, object data = null)
        {
            response.StatusCode = 201;
            if (string.IsNullOrWhiteSpace(location))
            {
                return Task.CompletedTask;
            }

            if (!response.Headers.ContainsKey(LocationHeader))
            {
                response.Headers.Add(LocationHeader, location);
            }

            return data is null ? Task.CompletedTask : response.WriteJsonAsync(data);
        }

        public static Task Accepted(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.Accepted;
            return Task.CompletedTask;
        }

        public static Task NoContent(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.NoContent;
            return Task.CompletedTask;
        }
        
        public static Task MovedPermanently(this HttpResponse response, string url)
        {
            response.StatusCode = (int) HttpStatusCode.MovedPermanently;
            if (!response.Headers.ContainsKey(LocationHeader))
            {
                response.Headers.Add(LocationHeader, url);
            }
            
            return Task.CompletedTask;
        }
        
        public static Task Redirect(this HttpResponse response, string url)
        {
            response.StatusCode = (int) HttpStatusCode.PermanentRedirect;
            if (!response.Headers.ContainsKey(LocationHeader))
            {
                response.Headers.Add(LocationHeader, url);
            }
            
            return Task.CompletedTask;
        }

        public static Task BadRequest(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.BadRequest;
            return Task.CompletedTask;
        }
        
        public static Task Unauthorized(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }
        
        public static Task Forbidden(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        }

        public static Task NotFound(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.NotFound;
            return Task.CompletedTask;
        }

        public static Task InternalServerError(this HttpResponse response)
        {
            response.StatusCode = (int) HttpStatusCode.InternalServerError;
            return Task.CompletedTask;
        }
    }
}