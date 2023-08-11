using SparkServerLite.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SparkServerLite.ViewModels
{
    public class BlogTagEditViewModel : BaseViewModel
    {
        public EditMode Mode { get; set; }

        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }

        public BlogTagEditViewModel()
        {
            Name = string.Empty;
        }
    }
}
