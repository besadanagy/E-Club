namespace E_Club.Models;

public class ClassBooking
{
    public int Id { get; set; }
    public int SportClassId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime BookedOn { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public virtual SportClass SportClass { get; set; } = default!;
    public virtual ApplicationUser User { get; set; } = default!;
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Waitlisted
}
