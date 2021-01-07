using Microsoft.AspNetCore.Http;
using Trill.Shared.Abstractions.Contexts;

namespace Trill.Shared.Infrastructure.Contexts
{
    internal sealed class ContextFactory : IContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IContext Create()
        {
            var context = _httpContextAccessor.HttpContext;
            
            return context is null ? Context.Empty : new Context(context);
        }
    }
}