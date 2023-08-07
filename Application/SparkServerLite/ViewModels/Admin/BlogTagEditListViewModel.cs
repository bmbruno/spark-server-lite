namespace SparkServerLite.ViewModels.Admin
{
    public class BlogTagListViewModel : BaseViewModel
    {
        public List<BlogTagListItemViewModel> BlogTagList { get; set; }

        public BlogTagListViewModel()
        {
            BlogTagList = new List<BlogTagListItemViewModel>();
        }
    }
}
