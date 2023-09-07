namespace SparkServerLite.SSO
{
    public class TokenPayload
    {
        public Guid uid { get; set; }

        public string eml { get; set; } = string.Empty;

        public string fname { get; set; } = string.Empty;

        public string lname { get; set; } = string.Empty;
    }
}
