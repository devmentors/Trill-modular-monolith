using System;
using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Modules
{
    public interface IModuleSubscriber
    {
        IModuleSubscriber Subscribe(string path, Func<object, Task<object>> action);
        
        IModuleSubscriber Subscribe<TRequest, TResponse>(string path, Func<TRequest, Task<TResponse>> action)
            where TRequest : class where TResponse : class;

        IModuleSubscriber SubscribeRequest<TRequest>(string path, Func<TRequest, Task<object>> action)
            where TRequest : class;

        IModuleSubscriber SubscribeResponse<TResponse>(string path, Func<object, Task<TResponse>> action)
            where TResponse : class;
    }
}
