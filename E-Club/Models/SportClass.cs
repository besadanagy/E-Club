namespace E_Club.Models;

public class SportClass : AuditTable
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int SportId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public ClassStatus Status { get; set; } = ClassStatus.Upcoming;
    public ClassType Type { get; set; } = ClassType.Regular;
    public decimal Price { get; set; }
    public int? CoachId { get; set; }

    public virtual Sport Sport { get; set; } = default!;
    public virtual Coach? Coach { get; set; }
    public virtual ICollection<ClassBooking> Bookings { get; set; } = new List<ClassBooking>();
}

public enum ClassStatus
{
    Upcoming,
    Ongoing,
    Completed,
    Cancelled
}

public enum ClassType
{
    Regular,
    Special
}
