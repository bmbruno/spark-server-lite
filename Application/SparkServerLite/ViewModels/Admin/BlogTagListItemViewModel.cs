namespace SparkServerLite.ViewModels.Admin
{
    public class BlogTagListItemViewModel : BaseViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Uses { get; set; }

        public BlogTagListItemViewModel()
        {
            Uses = 0;
        }
    }
}
