namespace Trill.Shared.Infrastructure.Vault
{
    internal class VaultOptions
    {
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string AuthType { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public KeyValueOptions Kv { get; set; }

        public class KeyValueOptions
        {
            public bool Enabled { get; set; }
            public int EngineVersion { get; set; } = 2;
            public string MountPoint { get; set; } = "kv";
            public string Path { get; set; }
            public int? Version { get; set; }
        }
    }
}