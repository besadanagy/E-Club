namespace E_Club.DTOs.Banners.Requests;

public record CreateBannerRequest(
    string Title,
    string Subtitle,
    string ImageUrl,
    string? ActionUrl,
    bool IsActive,
    int DisplayOrder,
    string Type
);
