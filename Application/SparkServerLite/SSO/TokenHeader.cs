using System.Text.Json.Serialization;

namespace SparkServerLite.SSO
{
    public class TokenHeader
    {

        public Guid id { get; set; }

        public int ver { get; set; }

        public DateTime exp { get; set; }

        [JsonIgnore]
        public string expString
        {
            get
            {
                return exp.ToString(format: "yyyyMMdd HHmmss");
            }
        }
    }
}
