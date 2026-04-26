namespace E_Club.Models;

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.General;

    // Target user (null = broadcast to all)
    public string? UserId { get; set; }

    // Read status
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    // Optional reference to related entity (Event, Sport, etc.)
    public int? ReferenceId { get; set; }
    public string? ReferenceType { get; set; } // "Event", "Sport", "Booking", etc.

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    // Navigation
    public virtual ApplicationUser? User { get; set; }
}

public enum NotificationType
{
    General,
    Event,
    Sport,
    Booking,
    System,
    Promotion
}
