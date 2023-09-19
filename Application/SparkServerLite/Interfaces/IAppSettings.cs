namespace SparkServerLite.Interfaces
{
    public interface IAppSettings
    {
        bool Debug { get; set; }

        string Sitename { get; set; }

        string SiteDescription { get; set; }

        string SiteURL { get; set; }

        string MediaFolderServerPath { get; set; }

        string MediaFolderWebPath { get; set; }

        string DatabaseConnectionString { get; set; }

        int BlogItemsPerPage { get; set; }

        string BlogBannerServerPath { get; set; }

        string BlogBannerWebPath { get; set; }

        string DefaultBlogBannerPath { get; set; }

        string DefaultBlogBannerThumbnailPath { get; set; }

        string SSOSigningKey { get; set; }

        string SSOSiteID { get; set; }

        string SSOLoginURL { get; set; }

        string SSOLogoutURL { get; set; }

    }
}
