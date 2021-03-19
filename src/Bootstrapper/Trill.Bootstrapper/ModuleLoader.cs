using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Trill.Shared.Infrastructure.Modules;

namespace Trill.Bootstrapper
{
    internal static class ModuleLoader
    {
        public static IList<Assembly> LoadAssemblies(IConfiguration configuration)
        {
            const string modulePart = "Trill.Modules.";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
                .ToList();

            var disabledModules = new List<string>();
            foreach (var file in files)
            {
                if (!file.Contains(modulePart))
                {
                    continue;
                }

                var moduleName = file.Split(modulePart)[1].Split(".")[0].ToLowerInvariant();
                var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");
                if (!enabled)
                {
                    disabledModules.Add(file);
                }
            }

            foreach (var disabledModule in disabledModules)
            {
                files.Remove(disabledModule);
            }
            
            files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

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