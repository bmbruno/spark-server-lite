using Microsoft.AspNetCore.Routing.Constraints;

namespace SparkServerLite.Models
{
    public class MediaItem
    {
        public string Filename { get; set; } = string.Empty;

        public string? Filetype { get;set;}

        public string? ServerPath { get; set; }

        public string? WebPath { get; set; }

        public string? ThumbnailPath { get; set; }
    }
}
