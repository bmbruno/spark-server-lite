using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Admin;
using System.Runtime;

namespace SparkServerLite.Mapping
{
    public static class MappingExtensions
    {

        /// <summary>
        /// Maps a list of blog articles to the HomeViewModel BlogList field.
        /// </summary>
        /// <param name="blogList">List of blogs from the database.</param>
        public static void MapToViewModel(this HomeViewModel vm, IEnumerable<Blog> blogList, IAppSettings settings)
        {
            foreach (var blog in blogList)
            {
                BlogViewModel blogVM = new BlogViewModel();
                blogVM.MapToViewModel(blog, null, settings);
                vm.BlogList.Add(blogVM);
            }
        }

        /// <summary>
        /// Maps a Blog model to a BlogViewModel.
        /// </summary>
        /// <param name="blog">Blog model.</param>
        /// <param name="blogTags">List of BlogTag models. Optional.</param>
        public static void MapToViewModel(this BlogViewModel vm, Blog blog, IEnumerable<BlogTag>? blogTags, IAppSettings settings)
        {
            vm.BlogID = blog.ID;
            vm.Slug = blog.Slug;
            vm.Title = blog.Title;
            vm.Subtitle = blog.Subtitle;
            vm.Content = blog.Content;
            vm.ImagePath = !String.IsNullOrEmpty(blog.ImagePath) ? blog.ImagePath : settings.DefaultBlogBannerPath;
            vm.ImageThumbnailPath = !String.IsNullOrEmpty(blog.ImageThumbnailPath) ? blog.ImageThumbnailPath : settings.DefaultBlogBannerThumbnailPath;
            vm.AuthorFullName = !string.IsNullOrEmpty(blog.AuthorFullName) ? blog.AuthorFullName : string.Empty;

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
        public static void MapToViewModel(this BlogListViewModel vm, IEnumerable<Blog> blogs, IEnumerable<BlogTag> tags, IAppSettings settings)
        {
            foreach (var blog in blogs)
            {
                BlogViewModel blogVM = new BlogViewModel();
                blogVM.MapToViewModel(blog, tags, settings);
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

        public static void MapToViewModel(this BlogEditListViewModel vm, IEnumerable<Blog> blogs)
        {
            foreach (var blog in blogs)
            {
                vm.BlogList.Add(new BlogListItemViewModel()
                {
                    ID = blog.ID,
                    Title = blog.Title,
                    Subtitle = blog.Subtitle,
                    PublishedDate = blog.PublishDate.ToShortDateString(),
                    AuthorName = !String.IsNullOrEmpty(blog.AuthorFullName) ? blog.AuthorFullName : "N/A"
                });
            }
        }

    }
}