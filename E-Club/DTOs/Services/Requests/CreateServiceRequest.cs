namespace E_Club.DTOs.Services.Requests;

public record CreateServiceRequest(
    string Name,
    string Description,
    string Icon,
    string Endpoint,
    string? ImageUrl,
    bool IsActive,
    int DisplayOrder,
    string Type
);