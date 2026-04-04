using E_Club.Models;

namespace E_Club.Models;

public class Event : AuditTable
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public EventStatus Status { get; set; }
}

public enum EventStatus
{
    Upcoming,
    Ongoing,
    Completed,
    Cancelled
}