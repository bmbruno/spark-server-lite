using SparkServerLite.Models;

namespace SparkServerLite.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public List<BlogViewModel> BlogList { get; set; }

        public HomeViewModel()
        {
            BlogList = new List<BlogViewModel>();
        }
    }
}
