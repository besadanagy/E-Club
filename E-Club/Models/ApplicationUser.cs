namespace E_Club.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDisabled { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? NationalId { get; set; }
        public string? MembershipId { get; set; } 
        public string? SequenceNumber { get; set; }
        public string? ClubCode { get; set; }      
        public string? DigitalAccessKey { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}