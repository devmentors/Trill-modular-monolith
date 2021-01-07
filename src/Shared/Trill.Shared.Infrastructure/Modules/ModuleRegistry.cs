using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trill.Shared.Abstractions;

namespace Trill.Shared.Infrastructure.Modules
{
    internal class ModuleRegistry : IModuleRegistry
    {
        private readonly ISet<string> _modules = new HashSet<string>();
        private readonly IDictionary<string, ModuleRequestRegistration> _requestActions;
        private readonly IList<ModuleBroadcastRegistration> _broadcastActions;

        public IEnumerable<string> Modules => _modules;
        
        public ModuleRegistry()
        {
            _broadcastActions = new List<ModuleBroadcastRegistration>();
            _requestActions = new Dictionary<string, ModuleRequestRegistration>();
        }

        public ModuleRequestRegistration GetRequestRegistration(string path)
            => _requestActions.TryGetValue(path, out var registration) ? registration : default; 

        public IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string path)
            => _broadcastActions.Where(r => r.Path == path);

        public bool TryAddRequestAction(string path, Type requestType, Type responseType,
            Func<object, Task<object>> action)
        {
            if (path == null)
            {
                throw new InvalidOperationException("Request path cannot be null.");
            }
            
            var registration = new ModuleRequestRegistration
            {
                RequestType = requestType,
                ResponseType = responseType,
                Action = action
            };

            return _requestActions.TryAdd(path, registration);
        }

        public void AddBroadcastAction(Type requestType, Func<object, Task> action)
        {
            if (string.IsNullOrWhiteSpace(requestType.Namespace))
            {
                throw new Exception("Missing namespace.");
            }
            
            var registration = new ModuleBroadcastRegistration
            {
                ReceiverType = requestType,
                Action = action,
                Module = requestType.GetModuleName()
            };

            _modules.Add(registration.Module);
            _broadcastActions.Add(registration);
        }
    }
}
