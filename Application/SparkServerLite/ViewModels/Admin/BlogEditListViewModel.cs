namespace SparkServerLite.ViewModels.Admin
{
    public class BlogEditListViewModel : BaseViewModel
    {
        public List<BlogListItemViewModel> BlogList { get; set; }

        public BlogEditListViewModel()
        {
            BlogList = new List<BlogListItemViewModel>();
        }
    }
}
