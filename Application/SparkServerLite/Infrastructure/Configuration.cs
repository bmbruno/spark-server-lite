using System.Runtime.CompilerServices;

namespace SparkServerLite.Infrastructure
{
    public static class Configuration
    {
        public static string SiteURL
        {
            get
            {
                return "https://blog.brandonbruno.com";
            }
        }

        /// <summary>
        /// Connection string to SQLite database file.
        /// </summary>
        public static string DatabaseConnectionString
        {
            get
            {
                return "Data Source=SparkServer.db";
            }
        }

        /// <summary>
        /// Number of Blog items that should display on a page.
        /// </summary>
        public static int BlogItemsPerPage
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// The default image to use for blog hero banners.
        /// </summary>
        public static string DefaultBlogBannerPath
        {
            get
            {
                return "/images/default-blog-image.png";
            }
        }

        /// <summary>
        /// The default image to use for blog thumbnails.
        /// </summary>
        public static string DefaultBlogBannerThumbnailPath
        {
            get
            {
                return "/images/default-blog-image.png";
            }
        }
    }
}
