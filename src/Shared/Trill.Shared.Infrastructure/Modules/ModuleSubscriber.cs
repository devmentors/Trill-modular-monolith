using System;
using System.Threading.Tasks;
using Trill.Shared.Abstractions.Modules;

namespace Trill.Shared.Infrastructure.Modules
{
    internal sealed class ModuleSubscriber : IModuleSubscriber
    {
        private readonly IModuleRegistry _registry;

        public ModuleSubscriber(IModuleRegistry registry)
            => _registry = registry;

        public IModuleSubscriber SubscribeRequest<TRequest>(string path, Func<TRequest, Task<object>> action)
            where TRequest : class
            => Subscribe(path, action);

        public IModuleSubscriber SubscribeResponse<TResponse>(string path, Func<object, Task<TResponse>> action)
            where TResponse : class
            => Subscribe(path, action);

        public IModuleSubscriber Subscribe(string path, Func<object, Task<object>> action)
            => Subscribe<object, object>(path, action);

        public IModuleSubscriber Subscribe<TRequest, TResponse>(string path, Func<TRequest, Task<TResponse>> action)
            where TRequest : class where TResponse : class
        {
            if (!_registry.TryAddRequestAction(path, typeof(TRequest), typeof(TResponse),
                async request => await action((TRequest) request)))
            {
                throw new InvalidOperationException($"Can't subscribe module path: '{path}'.");
            }

            return this;
        }
    }
}
