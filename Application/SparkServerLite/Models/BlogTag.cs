namespace SparkServerLite.Models
{
    public class BlogTag
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Uses { get; set; }

        public BlogTag()
        {
            Name = string.Empty;
            Uses = 0;
        }
    }
}