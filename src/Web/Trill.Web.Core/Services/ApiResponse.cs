using System.Net.Http;

namespace Trill.Web.Core.Services
{
    public class ApiResponse
    {
        public HttpResponseMessage HttpResponse { get; }
        public bool Succeeded { get; }
        public ErrorResponse Error { get; }

        public ApiResponse(HttpResponseMessage httpResponse, bool succeeded, ErrorResponse error = null)
        {
            HttpResponse = httpResponse;
            Succeeded = succeeded;
            Error = error;
        }

        public class ErrorResponse
        {
            public string Code { get; set; }
            public string Reason { get; set; }
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Value { get; }

        public ApiResponse(T value, HttpResponseMessage httpResponse, bool succeeded, ErrorResponse error = null)
            : base(httpResponse, succeeded, error)
        {
            Value = value;
        }
    }
}