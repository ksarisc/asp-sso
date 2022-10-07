namespace AspSso.Models
{
    public class TokenData
    {
        public string? Error { get; set; }

        public string? AccessToken { get; set; }
        public long ExpiresIn { get; set; } = 0;
        public string? Scope { get; set; }
        public string? TokenType { get; set; }
        public string? IdToken { get; set; }

        public static readonly TokenData Empty = new TokenData();
    }
}
