using SparkServerLite.Models.Analytics;

namespace SparkServerLite.ViewModels.Reports
{
    public class ReportPageViewsViewModel : BaseViewModel
    {
        public List<PageViewItem> ReportData { get; set; }

        public ReportPageViewsViewModel()
        {
            this.ReportData = new List<PageViewItem>();
        }
    }
}
