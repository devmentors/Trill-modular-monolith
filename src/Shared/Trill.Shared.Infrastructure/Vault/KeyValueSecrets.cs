using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp;

namespace Trill.Shared.Infrastructure.Vault
{
    internal sealed class KeyValueSecrets
    {
        private readonly IVaultClient _client;
        private readonly VaultOptions _options;

        public KeyValueSecrets(IVaultClient client, VaultOptions options)
        {
            _client = client;
            _options = options;
        }

        public async Task<T> GetAsync<T>(string path)
            => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(await GetAsync(path)));

        public async Task<IDictionary<string, object>> GetAsync(string path)
        {
            try
            {
                switch (_options.Kv.EngineVersion)
                {
                    case 1:
                        var secretV1 = await _client.V1.Secrets.KeyValue.V1.ReadSecretAsync(path,
                            _options.Kv.MountPoint);
                        return secretV1.Data;
                    case 2:
                        var secretV2 = await _client.V1.Secrets.KeyValue.V2.ReadSecretAsync(path,
                            _options.Kv.Version, _options.Kv.MountPoint);
                        return secretV2.Data.Data;
                    default:
                        throw new VaultException($"Invalid KV engine version: {_options.Kv.EngineVersion}.");
                }
            }
            catch (Exception exception)
            {
                throw new VaultException($"Getting Vault secret for path: '{path}' caused an error. " +
                                         $"{exception.Message}", exception, path);
            }
        }
    }
}