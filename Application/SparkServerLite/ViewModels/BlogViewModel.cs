using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SparkServerLite.ViewModels
{
    public class BlogViewModel : BaseViewModel
    {
        public int BlogID { get; set; }

        public bool IsPreview { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Content { get; set; }

        public string ImagePath { get; set; }

        public string ImageThumbnailPath { get; set; }

        public string AuthorFullName { get; set; }

        public DateTime PublishDate { get; set; }

        public List<BlogTagViewModel> BlogTags { get; set; }

        public string URL
        {
            get
            {
                return $"/blog/{this.PublishDate.Year}/{this.PublishDate.Month}/{this.Slug}";
            }
        }

        public string ImageURL
        {
            get
            {
                if (!String.IsNullOrEmpty(this.ImagePath))
                    return this.ImagePath;
                else
                    return "/Content/Images/default_blog_bg.jpg";
            }
        }

        public BlogViewModel()
        {
            BlogTags = new List<BlogTagViewModel>();
            MenuSelection = MainMenu.Home;

            Slug = string.Empty;
            Title = string.Empty;
            Subtitle = string.Empty;
            Content = string.Empty;
            ImagePath = string.Empty;
            ImageThumbnailPath = string.Empty;
            AuthorFullName = string.Empty;
        }

        /// <summary>
        /// Maps a Blog model to this view model.
        /// </summary>
        /// <param name="blog">Blog model.</param>
        /// <param name="blogTags">List of BlogTag models. Optional.</param>
        public void MapToViewModel(Blog blog, IEnumerable<BlogTag>? blogTags)
        {
            BlogID = blog.ID;
            Slug = blog.Slug;
            Title = blog.Title;
            Subtitle = blog.Subtitle;
            Content = blog.Content;
            ImagePath = blog.ImagePath;
            ImageThumbnailPath = blog.ImageThumbnailPath ?? "/Content/Images/default_blog_icon.png";
            AuthorFullName = String.IsNullOrEmpty(blog.AuthorFullName) ? blog.AuthorFullName : string.Empty;
            
            PublishDate = blog.PublishDate;

            if (blogTags != null)
            {
                foreach (var tag in blogTags)
                {
                    BlogTags.Add(new BlogTagViewModel()
                    {
                        BlogTagID = tag.ID,
                        BlogTagName = tag.Name
                    });
                }
            }
        }
    }
}
