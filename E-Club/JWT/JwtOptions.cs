using System.ComponentModel.DataAnnotations;

namespace E_Club.JWT
{
    public class JwtOptions
    {
        public const string SectionName = "JWT";

        [Required]
        public string Key { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty; // اول حرف كبير

        [Required]
        public string Audience { get; set; } = string.Empty; // اول حرف كبير

        [Range(1, int.MaxValue)]
        public int ExpiresInMinutes { get; set; } // اسم أوضح

        [Range(1, int.MaxValue)]
        public int RefreshTokenExpirationDays { get; set; }
    }
}