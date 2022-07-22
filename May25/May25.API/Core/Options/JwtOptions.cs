namespace May25.API.Core.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int ExpiresInDays { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
