namespace SparkServerLite.Models
{
    public class Filters
    {

        /// <summary>
        /// A single page of the site; only the URL (without the domain); example: "/blog/2025/this-is-a-post"
        /// </summary>
        public string Page { get; set; } = string.Empty;

        public Filters() { }

    }
}
