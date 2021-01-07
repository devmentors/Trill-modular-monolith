using System;
using System.Threading.Tasks;

namespace Trill.Shared.Infrastructure.Modules
{
    public sealed class ModuleRequestRegistration
    {
        public Type RequestType { get; set; }
        public Type ResponseType { get; set; }
        public Func<object, Task<object>> Action { get; set; }
    }
}
