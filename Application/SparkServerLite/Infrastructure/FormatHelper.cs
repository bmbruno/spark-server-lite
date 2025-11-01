using HtmlAgilityPack;
using Markdig;

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

        /// <summary>
        /// Converts a string of Markdown to valid HTML for hosting 
        /// </summary>
        /// <param name="markdownInput"></param>
        /// <returns></returns>
        public static string MarkdownToHTML(string markdownInput, string siteURL)
        {
            string htmlValue = Markdown.ToHtml(markdownInput);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlValue);

            htmlDoc = ExternalLinksGetBlankTarget(htmlDoc, siteURL);
            htmlDoc = FrameImagesWithFigure(htmlDoc);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// Adds target="_blank" attribute to all anchor tags that reference resources external to the domain name configured in AppSettings (use Config.DomainName in code to get at the value).
        /// </summary>
        /// <param name="htmlDoc">HtmlAgilityPack HtmlDocument object.</param>
        /// <returns>HtmlDocument object with anchor tags processed.</returns>
        public static HtmlDocument ExternalLinksGetBlankTarget(HtmlDocument htmlDoc, string siteURL)
        {
            var anchors = htmlDoc.DocumentNode.SelectNodes("//a");

            if (anchors == null)
                return htmlDoc;

            foreach (var anchor in anchors)
            {
                string originalHTML = anchor.OuterHtml;

                if (anchor.Attributes["href"] != null)
                {
                    string anchorValue = anchor.Attributes["href"].Value;

                    if (!anchorValue.Contains(siteURL) && !anchorValue.StartsWith("/"))
                    {
                        if (anchor.Attributes["target"] == null)
                        {
                            anchor.SetAttributeValue("target", "_blank");
                        }
                    }
                }
            }

            return htmlDoc;
        }

        /// <summary>
        /// Adds DIV with class of "figure" around standalone images. Markdown conversion will create "p/img", and this will convert that to "div/img".
        /// </summary>
        /// <param name="htmlDoc">HtmlAgilityPack HtmlDocument object.</param>
        /// <returns>HtmlDocument object with anchor tags processed.</returns>
        public static HtmlDocument FrameImagesWithFigure(HtmlDocument htmlDoc)
        {
            var images = htmlDoc.DocumentNode.SelectNodes("//img");

            if (images == null)
                return htmlDoc;

            foreach (var image in images)
            {
                if (image.ParentNode.Name.Equals("p"))
                {
                    image.ParentNode.Name = "div";
                    image.ParentNode.AddClass("figure full");
                }

                if (image.ParentNode.Name.Equals("a") && image.ParentNode.ParentNode.Name.Equals("p"))
                {
                    image.ParentNode.ParentNode.Name = "div";
                    image.ParentNode.ParentNode.AddClass("figure full");
                }
                
                if (image.ParentNode.Name.Equals("a"))
                    image.ParentNode.Attributes.Add("target", "_blank");
            }

            return htmlDoc;
        }
    }
}
