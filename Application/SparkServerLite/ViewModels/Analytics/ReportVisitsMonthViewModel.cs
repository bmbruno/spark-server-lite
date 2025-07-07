using SparkServerLite.Models.Analytics;

namespace SparkServerLite.ViewModels.Analytics
{
    public class ReportVisitsMonthViewModel : BaseViewModel
    {
        public List<VisitByMonthItem> ReportData { get; set; }

        public ReportVisitsMonthViewModel()
        {
            this.ReportData = new List<VisitByMonthItem>();
        }
    }
}
