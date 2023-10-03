using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure
{
    public class AppSettings : IAppSettings
    {
        public bool Debug { get; set; } = false;

        public string Sitename { get; set; } = string.Empty;

        public string SiteDescription { get; set; } = string.Empty;

        public string SiteURL { get; set; } = string.Empty;

        public string MediaFolderServerPath { get; set; } = string.Empty;

        public string MediaFolderWebPath { get; set; } = string.Empty;

        public string DatabaseConnectionString { get; set; } = string.Empty;

        public int BlogItemsPerPage { get; set; }

        public string BlogBannerServerPath { get; set; } = string.Empty;

        public string BlogBannerWebPath { get; set; } = string.Empty;

        public string DefaultBlogBannerPath { get; set; } = string.Empty;

        public string DefaultBlogBannerThumbnailPath { get; set; } = string.Empty;

        public string LibraryMediaServerPath { get; set; } = string.Empty;

        public string LibraryMediaWebPath { get; set; } = string.Empty;

        public string SSOSigningKey { get; set; } = string.Empty;

        public string SSOSiteID { get; set; } = string.Empty;

        public string SSOLoginURL { get; set; } = string.Empty;

        public string SSOLogoutURL { get; set; } = string.Empty;
    }
}
