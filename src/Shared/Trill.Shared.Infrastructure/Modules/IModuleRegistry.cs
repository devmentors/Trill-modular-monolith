using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trill.Shared.Infrastructure.Modules
{
    public interface IModuleRegistry
    {
        IEnumerable<string> Modules { get; }
        ModuleRequestRegistration GetRequestRegistration(string path);
        IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string path);
        bool TryAddRequestAction(string path, Type requestType, Type responseType, Func<object, Task<object>> action);
        void AddBroadcastAction(Type requestType, Func<object, Task> action);
    }
}
