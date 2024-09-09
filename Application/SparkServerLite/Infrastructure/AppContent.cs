using SparkServerLite.Interfaces;
using System.Runtime.CompilerServices;

namespace SparkServerLite.Infrastructure
{
    public class AppContent : IAppContent
    {
        public string SiteTitle { get; set; } = string.Empty;

        public string SiteSubtitle { get; set; } = string.Empty;

        public bool ShowSubtitle { get; set; } = true;

        public string SiteLogoURL { get; set; } = string.Empty;

        public IFooter Footer { get; set; }

        public AppContent()
        {
            Footer = new Footer();
        }
    }

    public class Footer : IFooter
    {
        public string Copyright { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Blurb { get; set; } = string.Empty;

    }
}