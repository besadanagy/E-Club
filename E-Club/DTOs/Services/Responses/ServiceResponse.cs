namespace E_Club.DTOs.Services.Responses;

public record ServiceResponse(
    int Id,
    string Name,
    string Description,
    string Icon,
    string Endpoint,
    string? ImageUrl,
    bool IsActive,
    int DisplayOrder,
    string Type
);