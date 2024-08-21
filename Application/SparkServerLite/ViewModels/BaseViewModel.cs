using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;

namespace SparkServerLite.ViewModels
{
    public class BaseViewModel
    {
        public MainMenu MenuSelection { get; set; }

        public Paging Paging { get; set; }

        public IAppContent AppContent { get; set; }

        public string SiteURL { get; set; } = string.Empty;

        public BaseViewModel()
        {
            Paging = new Paging();
            AppContent = new AppContent();
        }
    }
}
