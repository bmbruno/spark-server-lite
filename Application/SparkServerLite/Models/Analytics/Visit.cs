namespace SparkServerLite.Models.Analytics
{
    public class Visit
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string? UserAgent { get; set; } = string.Empty;

        public string? Domain { get; set; } = string.Empty;

        public string? Page { get; set; } = string.Empty;

        public string? Referer { get; set; } = string.Empty;

        public string? OS { get; set; } = string.Empty;

        public string? Device { get; set; } = string.Empty;

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
