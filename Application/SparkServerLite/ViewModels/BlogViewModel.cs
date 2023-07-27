using SparkServerLite.Infrastructure.Enums;
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

        public string Body { get; set; }

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
            Body = string.Empty;
            ImagePath = string.Empty;
            ImageThumbnailPath = string.Empty;
            AuthorFullName = string.Empty;
        }
    }
}
