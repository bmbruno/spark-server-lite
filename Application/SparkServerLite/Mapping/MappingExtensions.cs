using SparkServerLite.Models;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Mapping
{
    public static class MappingExtensions
    {

        /// <summary>
        /// Maps a list of blog articles to the HomeViewModel BlogList field.
        /// </summary>
        /// <param name="blogList">List of blogs from the database.</param>
        public static void MapToViewModel(this HomeViewModel vm, IEnumerable<Blog> blogList)
        {
            foreach (var blog in blogList)
            {
                BlogViewModel blogVM = new BlogViewModel();
                blogVM.MapToViewModel(blog, null);
                vm.BlogList.Add(blogVM);
            }
        }

        /// <summary>
        /// Maps a Blog model to a BlogViewModel.
        /// </summary>
        /// <param name="blog">Blog model.</param>
        /// <param name="blogTags">List of BlogTag models. Optional.</param>
        public static void MapToViewModel(this BlogViewModel vm, Blog blog, IEnumerable<BlogTag>? blogTags)
        {
            vm.BlogID = blog.ID;
            vm.Slug = blog.Slug;
            vm.Title = blog.Title;
            vm.Subtitle = blog.Subtitle;
            vm.Content = blog.Content;
            vm.ImagePath = blog.ImagePath;
            vm.ImageThumbnailPath = blog.ImageThumbnailPath ?? "/Content/Images/default_blog_icon.png"; // TODO: change to a config or static class field
            vm.AuthorFullName = string.IsNullOrEmpty(blog.AuthorFullName) ? blog.AuthorFullName : string.Empty;

            vm.PublishDate = blog.PublishDate;

            if (blogTags != null)
            {
                foreach (var tag in blogTags)
                {
                    vm.BlogTags.Add(new BlogTagViewModel()
                    {
                        BlogTagID = tag.ID,
                        BlogTagName = tag.Name
                    });
                }
            }
        }

        /// <summary>
        /// Database objects -> BlogListViewModel
        /// </summary>
        /// <param name="vm">BlogListViewModel</param>
        /// <param name="blogs">IEnumerable of Blog objects.</param>
        public static void MapToViewModel(this BlogListViewModel vm, IEnumerable<Blog> blogs, IEnumerable<BlogTag> tags)
        {
            foreach (var blog in blogs)
            {
                BlogViewModel blogVM = new BlogViewModel();
                blogVM.MapToViewModel(blog, tags);
                vm.BlogList.Add(blogVM);
            }

            foreach (var tag in tags)
            {
                BlogTagViewModel tagVM = new BlogTagViewModel();
                tagVM.MapToViewModel(tag);
                vm.BlogTagList.Add(tagVM);
            }
        }

        /// <summary>
        /// Database object -> BlogTagViewModel.
        /// </summary>
        /// <param name="vm">BlogTagViewModel</param>
        /// <param name="blogTag">Blog tag-type object.</param>
        public static void MapToViewModel(this BlogTagViewModel vm, BlogTag blogTag)
        {
            vm.BlogTagID = blogTag.ID;
            vm.BlogTagName = blogTag.Name;
        }

    }
}
