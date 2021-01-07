using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync<T>(T command) where T : class, ICommand;
    }
}