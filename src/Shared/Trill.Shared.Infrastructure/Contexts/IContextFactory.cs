using Trill.Shared.Abstractions.Contexts;

namespace Trill.Shared.Infrastructure.Contexts
{
    internal interface IContextFactory
    {
        IContext Create();
    }
}