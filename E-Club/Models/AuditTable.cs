namespace E_Club.Models
{
    public abstract class AuditTable : IAuditableEntity
    {
        [ForeignKey(nameof(CreatedBy))]
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UpdatedBy))]
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation Properties
        public virtual ApplicationUser CreatedBy { get; set; } = default!;
        public virtual ApplicationUser? UpdatedBy { get; set; }
    }
}