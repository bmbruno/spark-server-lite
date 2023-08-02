using SparkServerLite.Models;

namespace SparkServerLite.ViewModels
{
    public class BlogEditListViewModel : BaseViewModel
    {
        public List<Blog> BlogList { get; set; }

        public BlogEditListViewModel()
        {
            BlogList = new List<Blog>();
        } 
    }
}
