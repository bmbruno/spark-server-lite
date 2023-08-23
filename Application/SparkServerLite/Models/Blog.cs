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

        public string Title { get; set; } = string.Empty;

        public string? Subtitle { get; set; }

        public string Markdown { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public string? ImageThumbnailPath { get; set; }

        public string Slug { get; set; } = string.Empty;

        public string? MediaFolder { get; set; }

        public DateTime PublishDate { get; set; }

        public int AuthorID { get; set; }

        public string AuthorFullName { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }

        public Blog() { }

    }
}
