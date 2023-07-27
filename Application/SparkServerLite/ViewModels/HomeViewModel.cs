namespace SparkServerLite.ViewModels
{
    public class HomeViewModel
    {
        public List<BlogViewModel> BlogList { get; set; }

        public HomeViewModel()
        {
            BlogList = new List<BlogViewModel>();
        }
    }
}
