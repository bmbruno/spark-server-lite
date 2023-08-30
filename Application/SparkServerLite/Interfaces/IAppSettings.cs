namespace SparkServerLite.Interfaces
{
    public interface IAppSettings
    {
        string Sitename { get; set; }

        string SiteURL { get; set; }

        string MediaFolderServerPath { get; set; }

        string MediaFolderWebPath { get; set; }

        string DatabaseConnectionString { get; set; }

        int BlogItemsPerPage { get; set; }

        string BlogBannerServerPath { get; set; }

        string BlogBannerWebPath { get; set; }

        string DefaultBlogBannerPath { get; set; }

        string DefaultBlogBannerThumbnailPath { get; set; }

    }
}
