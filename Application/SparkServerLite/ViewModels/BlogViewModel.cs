using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SparkServerLite.ViewModels
{
    public class BlogViewModel : BaseViewModel
    {
        public int BlogID { get; set; }

        public bool IsPreview { get; set; }

        public string Slug { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

        public string ImageThumbnailPath { get; set; } = string.Empty;

        public string AuthorFullName { get; set; } = string.Empty;

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
        }
    }
}
