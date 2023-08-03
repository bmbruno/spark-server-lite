using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkServerLite.Models
{
    public class Blog
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string? Subtitle { get; set; }

        public string Content { get; set; }

        public string? ImagePath { get; set; }

        public string? ImageThumbnailPath { get; set; }

        public string Slug { get; set; }

        public DateTime PublishDate { get; set; }

        public int AuthorID { get; set; }

        public string AuthorFullName { get; set; }

        public Blog()
        {
            Title = string.Empty;
            Content = string.Empty;
            Slug = string.Empty;
            AuthorFullName = string.Empty;
        }

    }
}
