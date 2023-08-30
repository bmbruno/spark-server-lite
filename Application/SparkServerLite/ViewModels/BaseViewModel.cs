using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Models;

namespace SparkServerLite.ViewModels
{
    public class BaseViewModel
    {
        public MainMenu MenuSelection { get; set; }

        public Paging Paging { get; set; }

        public string Sitename { get; set; } = string.Empty;

        public string SiteURL { get; set; } = string.Empty;

        public BaseViewModel()
        {
            Paging = new Paging();
        }
    }
}
