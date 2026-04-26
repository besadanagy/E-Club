namespace E_Club.Models;

public class Sport : AuditTable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }

    public virtual ICollection<SportClass> Classes { get; set; } = new List<SportClass>();
}
