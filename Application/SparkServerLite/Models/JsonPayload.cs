using SparkServerLite.Infrastructure.Enums;

namespace SparkServerLite.Models
{
    public class JsonPayload
    {
        public string? Status { get; set; }

        public string? Message { get; set; }

        public object? Data { get; set; }

        public JsonPayload()
        {
            Status = null;
            Message = null;
            Data = null;
        }
    }
}
