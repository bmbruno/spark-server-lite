namespace SparkServerLite.Interfaces
{
    public interface IAppContent
    {
        /// <summary>
        /// Name (or title) of the website.
        /// </summary>
        string SiteName { get; set; }

        /// <summary>
        /// Description of the website; usuall displayed near the site logo or Sitename.
        /// </summary>
        string SiteDescription { get; set; }

    }
}
