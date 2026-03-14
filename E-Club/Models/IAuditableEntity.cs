namespace E_Club.Models
{
    public interface IAuditableEntity
    {
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation Properties
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}