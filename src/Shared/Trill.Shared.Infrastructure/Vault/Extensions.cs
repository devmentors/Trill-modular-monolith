using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace Trill.Shared.Infrastructure.Vault
{
    internal static class Extensions
    {
        private const string SectionName = "vault";

        public static IHostBuilder UseVault(this IHostBuilder builder, string sectionName = SectionName)
            => builder.ConfigureServices(services => services.AddVault(sectionName))
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    var options = cfg.Build().GetOptions<VaultOptions>(sectionName);
                    if (!options.Enabled)
                    {
                        return;
                    }

                    cfg.AddVaultAsync(options).GetAwaiter().GetResult();
                });
        
        public static IWebHostBuilder UseVault(this IWebHostBuilder builder, string sectionName = SectionName)
            => builder.ConfigureServices(services => services.AddVault(sectionName))
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    var options = cfg.Build().GetOptions<VaultOptions>(sectionName);
                    if (!options.Enabled)
                    {
                        return;
                    }

                    cfg.AddVaultAsync(options).GetAwaiter().GetResult();
                });

        private static IServiceCollection AddVault(this IServiceCollection services, string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }

            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<VaultOptions>(sectionName);
            VerifyOptions(options);
            services.AddSingleton(options);
            var (client, settings) = GetClientAndSettings(options);
            services.AddSingleton(settings);
            services.AddSingleton(client);

            return services;
        }
        
        
        private static void VerifyOptions(VaultOptions options)
        {
            if (options.Kv is null)
            {
                return;
            }

            if (options.Kv.EngineVersion > 2 || options.Kv.EngineVersion < 0)
            {
                throw new VaultException($"Invalid KV engine version: {options.Kv.EngineVersion} (available: 1 or 2).");
            }
                
            if (options.Kv.EngineVersion == 0)
            {
                options.Kv.EngineVersion = 2;
            }
        }

        private static async Task AddVaultAsync(this IConfigurationBuilder builder, VaultOptions options)
        {
            VerifyOptions(options);
            var (client, _) = GetClientAndSettings(options);
            if (options.Kv is null || !options.Kv.Enabled)
            {
                return;
            }

            var kvPath = options.Kv?.Path;
            if (string.IsNullOrWhiteSpace(kvPath))
            {
                throw new VaultException("Vault KV secret path can not be empty.");
            }
            
            Console.WriteLine($"Loading settings from Vault: '{options.Url}', KV path: '{kvPath}'.");
            var keyValueSecrets = new KeyValueSecrets(client, options);
            var secret = await keyValueSecrets.GetAsync(kvPath);
            var parser = new JsonParser();
            var data = parser.Parse(JObject.FromObject(secret));
            var source = new MemoryConfigurationSource {InitialData = data};
            builder.Add(source);
        }

        private static (IVaultClient client, VaultClientSettings settings) GetClientAndSettings(VaultOptions options)
        {
            var settings = new VaultClientSettings(options.Url, GetAuthMethod(options));
            var client = new VaultClient(settings);

            return (client, settings);
        }

        private static IAuthMethodInfo GetAuthMethod(VaultOptions options)
            => options.AuthType?.ToLowerInvariant() switch
            {
                "token" => new TokenAuthMethodInfo(options.Token),
                "userpass" => new UserPassAuthMethodInfo(options.Username, options.Password),
                _ => throw new VaultAuthTypeNotSupportedException(
                    $"Vault auth type: '{options.AuthType}' is not supported.", options.AuthType)
            };
    }
}