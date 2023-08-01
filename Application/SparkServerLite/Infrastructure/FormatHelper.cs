namespace SparkServerLite.Infrastructure
{
    public static class FormatHelper
    {
        public static string SQLiteDate = "yyyy-MM-dd";

        public static string SQLiteDateTime = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Formats a Tag name for a URL ("Alpha Tag" -> "alpha-tag").
        /// </summary>
        /// <param name="input">Tag name to format.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatTagForURL(string input)
        {
            return input.Replace(" ", "-");
        }

        /// <summary>
        /// Gets the database-friendly tag name from a URL value.
        /// </summary>
        /// <param name="input">Tag name from URL.</param>
        /// <returns>Unencoded string.</returns>
        public static string GetTagNameFromURL(string input)
        {
            return input.Replace("-", " ");
        }
    }
}
