namespace E_Club.Models
{
    public abstract class AuditTable : IAuditableEntity
    {
        [ForeignKey(nameof(CreatedBy))]
        public string? CreatedById { get; set; } = null;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UpdatedBy))]
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation Properties
        public virtual ApplicationUser? CreatedBy { get; set; } = null;
        public virtual ApplicationUser? UpdatedBy { get; set; }
    }
}