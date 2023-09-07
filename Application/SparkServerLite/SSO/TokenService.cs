using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SparkServerLite.SSO
{
    public static class TokenService
    {
        private static readonly int _tokenVersion = 1;

        /// <summary>
        /// Validates a token based on: valid signature and non-expired date.
        /// </summary>
        /// <param name="rawToken">Raw token.</param>
        /// <param name="signingSecret">Signing key.</param>
        /// <returns>TokenStatus enum; 'Valid' or one of several error types.</returns>
        public static TokenStatus ValidateToken(string rawToken, string signingKey)
        {
            // Token cannot be empty
            if (String.IsNullOrEmpty(rawToken))
                return TokenStatus.Empty;

            JsonToken token = Parse(rawToken);
            TokenHeader header = JsonConvert.DeserializeObject<TokenHeader>(token.Header);

            string tokenSig = GenerateSignature(token.HeaderEncoded, token.PayloadEncoded, signingKey);

            // Ensure signature matches
            if (!tokenSig.Equals(token.Signature, StringComparison.Ordinal))
                return TokenStatus.InvalidSignature;

            // Expiration date hasn't passed
            if (header.exp == null)
                return TokenStatus.InvalidHeader;

            if (header.exp <= DateTime.Now)
                return TokenStatus.Expired;

            return TokenStatus.Valid;
        }

        /// <summary>
        /// Validates a token based on: valid signature and non-expired date. Throws detailed exceptions instead of returning false.
        /// </summary>
        /// <param name="rawToken"></param>
        /// <param name="signingKey"></param>
        /// <param name="rawToken">Raw token.</param>
        /// <param name="signingSecret">Signing key.</param>
        /// <returns>TokenStatus enum; 'Valid' or one of several error types. Throws detailed exceptions for all false conditions.</returns>
        public static TokenStatus ValidateTokenDebug(string rawToken, string signingKey)
        {
            // Token cannot be empty
            if (String.IsNullOrEmpty(rawToken))
                throw new Exception("Token is empty. String.IsNullOrEmpty evaluated an empty token.");

            JsonToken token = Parse(rawToken);
            TokenHeader header = JsonConvert.DeserializeObject<TokenHeader>(token.Header);

            string tokenSig = GenerateSignature(token.HeaderEncoded, token.PayloadEncoded, signingKey);

            // Ensure signature matches
            if (!tokenSig.Equals(token.Signature, StringComparison.Ordinal))
                throw new Exception($"Signatures don't match. TOKEN SIG: {token.Signature} | COMPUTED SIG: {tokenSig}");

            // Expiration date hasn't passed
            if (header.exp == null)
                throw new Exception($"Header expiration date is empty. VALUE: {header.exp}");

            if (header.exp <= DateTime.Now)
                throw new Exception($"Header expiration date has passed. VALUE: {header.exp}");

            return TokenStatus.Valid;
        }

        /// <summary>
        /// Returns a strongly-typed model of the token payload data.
        /// </summary>
        /// <param name="rawToken">Raw token.</param>
        /// <returns>TokenPayload object.</returns>
        public static TokenPayload GetPayload(string rawToken)
        {
            if (String.IsNullOrEmpty(rawToken))
                throw new Exception("Token is empty.");

            JsonToken token = Parse(rawToken);
            TokenPayload payload = JsonConvert.DeserializeObject<TokenPayload>(token.Payload);

            return payload;
        }

        /// <summary>
        /// Creates a JsonToken that is ready for transit.
        /// </summary>
        /// <param name="payload">Strongly-typed TokenPayload object.</param>
        /// <param name="expiration">DateTime this token should expire</param>
        /// <param name="signingKey">Secret key used to sign this token.</param>
        /// <returns></returns>
        public static JsonToken CreateJsonToken(TokenPayload payload, DateTime expiration, string signingKey)
        {
            TokenHeader header = new TokenHeader()
            {
                id = Guid.NewGuid(),
                ver = _tokenVersion,
                exp = expiration
            };

            JsonToken jsonToken = new JsonToken();

            jsonToken.Header = JsonConvert.SerializeObject(header);
            jsonToken.HeaderEncoded = EncodeBase64(jsonToken.Header);
            jsonToken.Payload = JsonConvert.SerializeObject(payload);
            jsonToken.PayloadEncoded = EncodeBase64(jsonToken.Payload);
            jsonToken.Signature = GenerateSignature(jsonToken.HeaderEncoded, jsonToken.PayloadEncoded, signingKey);

            return jsonToken;
        }

        /// <summary>
        /// Parses a raw token string into a strongly-typed Token object.
        /// </summary>
        /// <param name="token">Raw token string.</param>
        /// <returns>Token object.</returns>
        private static JsonToken Parse(string token)
        {
            JsonToken newToken = new JsonToken();

            // Split on delimiter (.)
            string[] chunks = token.Split('.');

            if (chunks.Length != 3)
                throw new Exception("Raw token is malformed; incorrect number of chunks.");

            newToken.HeaderEncoded = chunks[0];
            newToken.PayloadEncoded = chunks[1];
            newToken.Signature = chunks[2];

            newToken.Header = DecodeBase64(newToken.HeaderEncoded);
            newToken.Payload = DecodeBase64(newToken.PayloadEncoded);

            return newToken;
        }

        /// <summary>
        /// Encodes a string to base-64.
        /// </summary>
        /// <param name="input">String to encode.</param>
        /// <returns>Base-64 encoded string.</returns>
        private static string EncodeBase64(string input)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Decodes a base-64 encoded value to a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string DecodeBase64(string input)
        {
            byte[] buffer = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Returns a JWT-like signature hash based on the base-64-encoded header and payload content of the token.
        /// </summary>
        /// <param name="headerBase64">Base-64-encoded header content.</param>
        /// <param name="payloadBase64">Base-64-encoded payload content.</param>
        /// <param name="signingKey">Client key secret.</param>
        /// <returns></returns>
        private static string GenerateSignature(string headerBase64, string payloadBase64, string signingKey)
        {
            string input = $"{headerBase64}{payloadBase64}{signingKey}";

            Encoding enc = Encoding.UTF8;
            SHA256 hasher = SHA256.Create();
            StringBuilder output = new StringBuilder();
            byte[] bytes = hasher.ComputeHash(enc.GetBytes(input));

            foreach (byte item in bytes)
                output.Append(item.ToString("x2"));

            return output.ToString();
        }
    }
}
