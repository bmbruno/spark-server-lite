using SparkServerLite.Infrastructure.Enums;

namespace SparkServerLite.Models
{
    public class JsonPayload
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public JsonPayload()
        {
            Message = string.Empty;
            Data = new List<dynamic>();
        }
    }
}
