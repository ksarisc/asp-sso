namespace AspSso.Models
{
    public class TokenData
    {
        public string? Data { get; set; }

        public static readonly TokenData Empty = new TokenData();
    }
}
