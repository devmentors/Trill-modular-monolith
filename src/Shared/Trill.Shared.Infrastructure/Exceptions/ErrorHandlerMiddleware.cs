using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Trill.Shared.Infrastructure.Exceptions
{
    internal sealed class ErrorHandlerMiddleware : IMiddleware
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(new CamelCaseNamingStrategy())
            }
        };

        private readonly IExceptionToResponseMapper _exceptionToResponseMapper;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(IExceptionToResponseMapper exceptionToResponseMapper,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _exceptionToResponseMapper = exceptionToResponseMapper;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var exceptionResponse = _exceptionToResponseMapper.Map(exception);
            context.Response.StatusCode = (int) (exceptionResponse?.StatusCode ?? HttpStatusCode.InternalServerError);
            var response = exceptionResponse?.Response;
            if (response is null)
            {
                await context.Response.WriteAsync(string.Empty);
                return;
            }

            context.Response.ContentType = "application/json";
            var payload = JsonConvert.SerializeObject(exceptionResponse.Response, SerializerSettings);
            await context.Response.WriteAsync(payload);
        }
    }
}