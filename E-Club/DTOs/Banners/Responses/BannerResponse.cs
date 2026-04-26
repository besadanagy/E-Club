namespace E_Club.DTOs.Banners.Responses;

public record BannerResponse(
    int Id,
    string Title,
    string Subtitle,
    string ImageUrl,
    string? ActionUrl,
    string Type
);
