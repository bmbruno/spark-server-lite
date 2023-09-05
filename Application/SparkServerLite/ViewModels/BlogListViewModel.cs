using SparkServerLite.Infrastructure.Enums;

namespace SparkServerLite.ViewModels
{
    public class BlogListViewModel : BaseViewModel
    {
        public List<BlogViewModel> BlogList { get; set; }

        public List<BlogTagViewModel> BlogTagList { get; set; }

        public string Header { get; set; }

        public BlogListViewModel()
        {
            BlogList = new List<BlogViewModel>();
            BlogTagList = new List<BlogTagViewModel>();
            MenuSelection = MainMenu.Blog;
            Header = string.Empty;
        }
    }
}
