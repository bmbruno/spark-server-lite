using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure
{
    public class AppSettings : IAppSettings
    {
        public bool Debug { get; set; } = false;

        public string DatabaseConnectionString { get; set; } = string.Empty;

        public string AnalyticsConnectionString { get; set; } = string.Empty;

        public string LogFolder { get; set; } = string.Empty;

        public string SiteURL { get; set; } = string.Empty;

        public int BlogItemsPerPage { get; set; }

        public string ServerWWWRoot { get; set; } = string.Empty;

        public string MediaFolderPath { get; set; } = string.Empty;

        public string BlogBannerPath { get; set; } = string.Empty;

        public string LibraryMediaPath { get; set; } = string.Empty;

        public string DefaultBlogBannerPath { get; set; } = string.Empty;

        public string DefaultBlogBannerThumbnailPath { get; set; } = string.Empty;

        public string SSOSigningKey { get; set; } = string.Empty;

        public string SSOSiteID { get; set; } = string.Empty;

        public string SSOLoginURL { get; set; } = string.Empty;

        public string SSOLogoutURL { get; set; } = string.Empty;
    }
}