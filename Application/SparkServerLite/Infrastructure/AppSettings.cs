using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure
{
    public class AppSettings : IAppSettings
    {
        public string SiteURL { get; set; } = string.Empty;

        public string MediaFolderRootPath { get; set; } = string.Empty;

        public string DatabaseConnectionString { get; set; } = string.Empty;

        public int BlogItemsPerPage { get; set; }

        public string DefaultBlogBannerPath { get; set; } = string.Empty;

        public string DefaultBlogBannerThumbnailPath { get; set; } = string.Empty;   
    }
}
