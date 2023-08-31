namespace SparkServerLite.ViewModels.Admin
{
    public class BlogListItemViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty; 

        public string PublishedDate { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public BlogListItemViewModel() { }
    }
}
