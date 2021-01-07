using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task HandleAsync(TCommand command);
    }
}