namespace E_Club.JWT
{
    public class JwtOptions
    {
        public const string SectionName = "JWT";

        [Required]
        public string Key { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string[] Audience { get; set; } = Array.Empty<string>();  // ◀️ بقيت array

        [Range(1, int.MaxValue)]
        public int ExpiresInMinutes { get; set; }

        [Range(1, int.MaxValue)]
        public int RefreshTokenExpirationDays { get; set; }
    }
}