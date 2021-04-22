using System.Threading.Tasks;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Abstractions.Modules
{
    public interface IModuleClient
    {
        Task PublishAsync(IMessage message);
        Task SendAsync(string path, object request);
        Task<TResult> SendAsync<TResult>(string path, object request) where TResult : class;
    }
}
