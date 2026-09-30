using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure;

public class SeoManager
{
    private readonly IAppSettings _settings;
    private readonly IWebHostEnvironment _host;

    private readonly string _robotsFilePath;

    public SeoManager(IAppSettings settings, IWebHostEnvironment host)
    {
        _settings = settings;
        _host = host;
        
        _robotsFilePath = Path.Combine(_host.WebRootPath, "robots.txt");
    }

    /// <summary>
    /// Writes the provided contents of a robots.txt file to the actual robots.txt on disk (in the application root).
    /// </summary>
    /// <param name="contents">Contents of the robots.txt file to save to disk.</param>
    /// <exception cref="Exception">Exceptions for null 'host' or file write operations.</exception>
    public void SaveRobotsTxtToDisk(string contents)
    {
        File.WriteAllText(_robotsFilePath, contents);
    }
}