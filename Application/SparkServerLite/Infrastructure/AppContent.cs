using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure
{
    public class AppContent : IAppContent
    {
        public string SiteName { get; set; } = string.Empty;

        public string SiteDescription { get; set; } = string.Empty;

    }
}