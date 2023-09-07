namespace SparkServerLite.SSO
{
    public class JsonToken
    {
        public string Header { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public string HeaderEncoded { get; set; } = string.Empty;

        public string PayloadEncoded { get; set; } = string.Empty;

        public string Signature { get; set; } = string.Empty;

        public string Raw
        {
            get
            {
                return $"{HeaderEncoded}.{PayloadEncoded}.{Signature}";
            }
        }
    }
}
