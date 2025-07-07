using SparkServerLite.Models.Analytics;

namespace SparkServerLite.ViewModels.Reports
{
    public class ReportVisitsMonthViewModel : BaseViewModel
    {
        public List<VIsitByMonthItem> ReportData { get; set; }

        public ReportVisitsMonthViewModel()
        {
            this.ReportData = new List<VIsitByMonthItem>();
        }
    }
}
