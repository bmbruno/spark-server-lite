namespace SparkServerLite.Interfaces
{
    public interface IAppSettings
    {
        /// <summary>
        /// Enables additional logging features. Also disables SSO - use with caution.
        /// </summary>
        bool Debug { get; set; }

        /// <summary>
        /// Connection string to the SQLite database.
        /// </summary>
        string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Connection string to the analytics SQLite database.
        /// </summary>
        string AnalyticsConnectionString { get; set; }

        /// <summary>
        /// Base URL of the site. Used to generate some URLs. Example: https://www.myblog.com
        /// </summary>
        string SiteURL { get; set; }

        /// <summary>
        /// Number of blog items to display on the Blog List page.
        /// </summary>
        int BlogItemsPerPage { get; set; }

        /// <summary>
        /// Root path of the website assets; usually 'wwwroot/' with the default ASP.NET Core web project templates.
        /// </summary>
        string ServerWWWRoot { get; set; }

        /// <summary>
        /// Relative path (from the root of the application) to the folder where blog post media should be stored. Example: /media
        /// </summary>
        string MediaFolderPath { get; set; }

        /// <summary>
        /// Relative path (from the root of the application) to the folder where blog banner images are stored. Example: /images/banners
        /// </summary>
        string BlogBannerPath { get; set; }

        /// <summary>
        /// Relative path (from the root of the application) to the folder where library images are stored. Example: /images/library
        /// </summary>
        string LibraryMediaPath { get; set; }

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
