using System.Text.Json.Serialization;

namespace AspSso.Models.Sso
{
    public class GoogleToken
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; } = 0;
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        public TokenData ToTokenData()
        {
            return new TokenData
            {
                AccessToken = AccessToken,
                ExpiresIn = ExpiresIn,
                Scope = Scope,
                TokenType = TokenType,
                IdToken = IdToken,
            };
        }
    }
}
