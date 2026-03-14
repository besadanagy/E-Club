namespace E_Club.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && DateTime.UtcNow <= ExpiresOn;

        // Navigation Property
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = default!;
    }
}