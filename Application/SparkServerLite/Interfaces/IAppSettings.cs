namespace SparkServerLite.Interfaces
{
    public interface IAppSettings
    {
        string SiteURL { get; set; }

        string MediaFolderServerPath { get; set; }

        string MediaFolderWebPath { get; set; }

        string DatabaseConnectionString { get; set; }

        int BlogItemsPerPage { get; set; }

        string DefaultBlogBannerPath { get; set; }

        string DefaultBlogBannerThumbnailPath { get; set; }

    }
}
