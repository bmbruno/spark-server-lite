using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure;

public class SeoManager
{
    private readonly IAppSettings _settings;
    private readonly IWebHostEnvironment _host;

    public SeoManager(IAppSettings settings, IWebHostEnvironment host)
    {
        _settings = settings;
        _host = host;
    } 
}