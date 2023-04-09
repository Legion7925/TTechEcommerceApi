namespace TTechEcommerceApi.Helper
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Secret { get; set; } = string.Empty;

        public int RefreshTokenTTL { get; set; }

        public string? Audience { get; set; }

        public string? Issuer { get; set; }
    }
}
