namespace E_Club.Models;

public class Banner : AuditTable
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public BannerType Type { get; set; }
}

public enum BannerType
{
    Tournament,
    Event,
    Promotion,
    News
}
