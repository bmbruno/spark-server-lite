namespace SparkServerLite.SSO
{
    public enum TokenStatus
    {
        Valid,
        InvalidHeader,
        InvalidPayload,
        InvalidSignature,
        Empty,
        Expired
    }
}
