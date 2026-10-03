using System.Text;
using System.Xml;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;

namespace SparkServerLite.Infrastructure;

public class SeoManager
{
    private readonly IAppSettings _settings;
    private readonly IWebHostEnvironment _host;

    private readonly string _robotsFilePath;
    private readonly string _sitemapFilePath;

    public SeoManager(IAppSettings settings, IWebHostEnvironment host)
    {
        _settings = settings;
        _host = host;
        
        _robotsFilePath = Path.Combine(_host.WebRootPath, "robots.txt");
        _sitemapFilePath = Path.Combine(_host.WebRootPath, "sitemap.xml");
    }

    /// <summary>
    /// Validates that robots.txt exists; creates it if it doesn't.
    /// </summary>
    private void ValidateRobotsTxt()
    {
        if (!File.Exists(_robotsFilePath))
            File.Create(_robotsFilePath).Dispose();
    }
    
    /// <summary>
    /// Reads the contents of robots.txt on disk.
    /// </summary>
    /// <returns>Contents of robots.txt as a string.</returns>
    public string LoadRobotsTxtFromDisk()
    {
        ValidateRobotsTxt();
        return File.ReadAllText(_robotsFilePath);
    }
    
    /// <summary>
    /// Writes the provided contents of a robots.txt file to the actual robots.txt on disk (in the application root).
    /// </summary>
    /// <param name="contents">Contents of the robots.txt file to save to disk.</param>
    public void SaveRobotsTxtToDisk(string contents)
    {
        File.WriteAllText(_robotsFilePath, contents);
    }

    /// <summary>
    /// Validates that sitemap.xml exists; creates it if it doesn't.
    /// </summary>
    private void ValidateSitemapFile()
    {
        if (!File.Exists(_sitemapFilePath))
            File.Create(_sitemapFilePath).Dispose();
    }
    
    /// <summary>
    /// Reads the contents of sitemap.xml file on disk.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> LoadSitemapFromDisk()
    {
        ValidateSitemapFile();
        return File.ReadAllLines(_sitemapFilePath);
    }

    /// <summary>
    /// Writes the provided contents of the sitemap to disk (in the application root). 
    /// </summary>
    /// <param name="contents"></param>
    public void SaveSitemapToDisk(IEnumerable<string> contents)
    {
        File.WriteAllLines(_sitemapFilePath, contents);
    }

    public void RebuildSitemap(IBlogRepository<Blog> blogRepo)
    {
        // TODO: rebuild complete sitemap XML structure
        // https://www.sitemaps.org/protocol.html

        IEnumerable<Blog> blogItems = blogRepo.GetAllPublishedBlogsForSitemap();
        
        if (blogItems == null) throw new ArgumentNullException(nameof(blogItems));

        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "  ",
            OmitXmlDeclaration = false
        };

        using (var writer = XmlWriter.Create(_sitemapFilePath, settings))
        {
            writer.WriteStartDocument();

            // <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (var item in blogItems)
            {
                if (string.IsNullOrWhiteSpace(item.URL)) continue;

                writer.WriteStartElement("url");

                // TODO: prepend site URL (blog.brandonbruno.com) from settings
                
                // <loc> - required
                writer.WriteElementString("loc", item.URL);

                // <lastmod> - optional (W3C Datetime format: YYYY-MM-DD or YYYY-MM-DDThh:mm:ssTZD)
                writer.WriteElementString("lastmod", item.PublishDate.ToString("yyyy-MM-ddTHH:mm:sszzz"));

                writer.WriteEndElement(); // </url>
            }

            writer.WriteEndElement(); // </urlset>
            writer.WriteEndDocument();
        }
    }
}