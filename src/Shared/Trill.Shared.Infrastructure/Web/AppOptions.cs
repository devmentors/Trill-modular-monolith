namespace Trill.Shared.Infrastructure.Web
{
    internal class AppOptions
    {
        public string Name { get; set; }
        public string Instance { get; set; }
        public string Version { get; set; }
        public bool DisplayBanner { get; set; } = true;
        public bool DisplayVersion { get; set; } = true;
    }
}