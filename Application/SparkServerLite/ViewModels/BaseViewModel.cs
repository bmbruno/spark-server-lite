using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Models;

namespace SparkServerLite.ViewModels
{
    public class BaseViewModel
    {
        public MainMenu MenuSelection { get; set; }

        public Paging Paging { get; set; }

        public BaseViewModel()
        {
            Paging = new Paging();
        }
    }
}
