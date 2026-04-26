namespace E_Club.Models;

public class EventRegistration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Confirmed;

    public virtual Event Event { get; set; } = default!;
    public virtual ApplicationUser User { get; set; } = default!;
}

public enum RegistrationStatus
{
    Confirmed,
    Cancelled,
    Waitlisted
}
