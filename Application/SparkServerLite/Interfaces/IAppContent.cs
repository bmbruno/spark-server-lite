namespace SparkServerLite.Interfaces
{
    public interface IAppContent
    {
        /// <summary>
        /// Name (or title) of the website.
        /// </summary>
        string SiteTitle { get; set; }

        /// <summary>
        /// Description of the website; usuall displayed near the site logo or Sitename.
        /// </summary>
        string SiteSubtitle { get; set; }

        /// <summary>
        /// URL of the site's logo image.
        /// </summary>
        string SiteLogoURL { get; set; }

        /// <summary>
        /// Content rendered in the site footer.
        /// </summary>
        IFooter Footer { get; set; }
    }

    public interface IFooter
    {
        /// <summary>
        /// Copyright string; example: Brandon Bruno; all rights reserved.
        /// </summary>
        string Copyright { get; set; }

        /// <summary>
        /// Description of the website.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Message, summary, overview, or other text about the website. Appears in right column of footer.
        /// </summary>
        string Blurb { get; set; }

    }
}
