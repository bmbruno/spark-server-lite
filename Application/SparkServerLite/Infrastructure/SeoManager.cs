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
}