using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Shared.Bootstrapper
{
    internal static class ModuleBootstrapper
    {
        public static IList<Assembly> LoadAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var locations = assemblies.Where(a => !a.IsDynamic).Select(a => a.Location).ToArray();
            Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
                .ToList()
                .ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

            return assemblies;
        }
        
        public static IList<IModule> LoadModules(IEnumerable<Assembly> assemblies)
        {
            var moduleType = typeof(IModule);
            var modules = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => moduleType.IsAssignableFrom(x) && !x.IsInterface)
                .OrderBy(x => x.Name)
                .Select(Activator.CreateInstance)
                .Cast<IModule>()
                .ToList();

            ValidateModules(modules);

            return modules;
        }

        private static void ValidateModules(IEnumerable<IModule> modules)
        {
            var duplicatedModulePaths = modules
                .Where(x => !string.IsNullOrWhiteSpace(x.Path))
                .GroupBy(x => x.Path)
                .Where(x => x.Count() > 1)
                .Select(x => $"'/{x.Key}'")
                .ToArray();
            if (duplicatedModulePaths.Any())
            {
                throw new Exception($"Duplicated module paths: {string.Join(",", duplicatedModulePaths)}");
            }
        }
    }
}