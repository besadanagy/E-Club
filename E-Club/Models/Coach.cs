namespace E_Club.Models;

public class Coach : AuditTable
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty; // "Football", "Swimming", etc.
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
    public int ExperienceYears { get; set; }
    public double Rating { get; set; } = 0.0;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual ICollection<SportClass> Classes { get; set; } = new List<SportClass>();
}
