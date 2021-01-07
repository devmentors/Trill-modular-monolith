using System.Threading.Tasks;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Abstractions.Modules
{
    public interface IModuleClient
    {
        Task<TResult> RequestAsync<TResult>(string path, object request) where TResult : class;
        Task SendAsync(IMessage message);
    }
}
