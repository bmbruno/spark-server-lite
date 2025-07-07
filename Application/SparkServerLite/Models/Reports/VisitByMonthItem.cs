namespace SparkServerLite.Models.Reports;

public class VisitByMonthItem
{
    public int Month { get; set; }

    public string MonthLabel { get; set; } = string.Empty;
    
    public int Visits { get; set; }
}