namespace SparkServerLite.Interfaces
{
    public interface IAppSettings
    {
        /// <summary>
        /// Enables additional logging features. Also disables SSO - use with caution.
        /// </summary>
        bool Debug { get; set; }

        /// <summary>
        /// Name of website; will be displayed to users.
        /// </summary>
        string Sitename { get; set; }

        /// <summary>
        /// Description of the website; usuall displayed near the site logo or Sitename.
        /// </summary>
        string SiteDescription { get; set; }

        /// <summary>
        /// Base URL of the site. Used to generate some URLs. Example: https://www.myblog.com
        /// </summary>
        string SiteURL { get; set; }

        /// <summary>
        /// Relative path (from the root of the application) to the folder where blog post media should be stored. Example: ./wwwroot/media
        /// </summary>
        string MediaFolderServerPath { get; set; }

        /// <summary>
        /// Absolute path to the wwwroot folder where browsers can request blog media files. Example: /media
        /// </summary>
        string MediaFolderWebPath { get; set; }

        /// <summary>
        /// Connection string to the SQLite database.
        /// </summary>
        string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Number of blog items to display on the Blog List page.
        /// </summary>
        int BlogItemsPerPage { get; set; }

        /// <summary>
        /// Relative path (from the root of the application) to the folder where default blog banner images are stored. Example: ./wwwroot/images/banners
        /// </summary>
        string BlogBannerServerPath { get; set; }

        /// <summary>
        /// Absolute path to the wwwroot folder where browsers can access default blog banner images. Example: /images/banners
        /// </summary>
        string BlogBannerWebPath { get; set; }

        /// <summary>
        /// Absolute WWW path to the default blog banner image. Example: /images/default_blog_banner.jpg
        /// </summary>
        string DefaultBlogBannerPath { get; set; }

        /// <summary>
        /// Absolute WWW path to the default blog banner thumbnail image. Example: /images/default_blog_banner_thumb.jpg
        /// </summary>
        string DefaultBlogBannerThumbnailPath { get; set; }

        /// <summary>
        /// Only used with QuickSSO system.
        /// </summary>
        string SSOSigningKey { get; set; }

        /// <summary>
        /// Only used with QuickSSO system.
        /// </summary>
        string SSOSiteID { get; set; }

        /// <summary>
        /// Only used with QuickSSO system.
        /// </summary>
        string SSOLoginURL { get; set; }

        /// <summary>
        /// Only used with QuickSSO system.
        /// </summary>
        string SSOLogoutURL { get; set; }

    }
}
