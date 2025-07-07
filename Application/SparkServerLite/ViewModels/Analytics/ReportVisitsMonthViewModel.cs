using SparkServerLite.Models.Analytics;

namespace SparkServerLite.ViewModels.Analytics
{
    public class ReportVisitsMonthViewModel : BaseViewModel
    {
        public string PageFilter { get; set; } = string.Empty;

        public List<VisitByMonthItem> ReportData { get; set; }

        public ReportVisitsMonthViewModel()
        {
            this.ReportData = new List<VisitByMonthItem>();
        }
    }
}
