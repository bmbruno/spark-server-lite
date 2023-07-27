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

        /// <summary>
        /// Maps a list of blog articles to the HomeViewModel BlogList field.
        /// </summary>
        /// <param name="blogList">List of blogs from the database.</param>
        public void MapToViewModel(IEnumerable<Blog> blogList)
        {
            foreach (var blog in blogList)
            {
                BlogViewModel blogVM = new BlogViewModel();
                blogVM.MapToViewModel(blog, null);
                BlogList.Add(blogVM);
            }
        }
    }
}
