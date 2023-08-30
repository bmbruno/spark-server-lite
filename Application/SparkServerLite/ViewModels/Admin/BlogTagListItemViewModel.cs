namespace SparkServerLite.ViewModels.Admin
{
    public class BlogTagListItemViewModel : BaseViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Uses { get; set; }

        public BlogTagListItemViewModel()
        {
            Name = string.Empty;
            Uses = 0;
        }
    }
}
