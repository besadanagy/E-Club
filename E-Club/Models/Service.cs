
namespace E_Club.Models;

public class Service : AuditTable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // اسم الأيقونة (FontAwesome, Material Icons)
    public string Endpoint { get; set; } = string.Empty; // /book-field, /join-tournament
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public ServiceType Type { get; set; }
}

public enum ServiceType
{
    Booking,
    Tournament,
    Coaching,
    Restaurant,
    Party,
    Other
}