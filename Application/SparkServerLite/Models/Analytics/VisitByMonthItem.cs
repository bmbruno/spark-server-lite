namespace SparkServerLite.Models.Analytics;

public class VisitByMonthItem
{
    public int Month { get; set; }

    public string MonthLabel { get; set; } = string.Empty;

    public int Year { get; set; }

    public int Visits { get; set; }
}