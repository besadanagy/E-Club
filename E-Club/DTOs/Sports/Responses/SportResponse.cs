namespace E_Club.DTOs.Sports.Responses;

public record SportResponse(
    int Id,
    string Name,
    string Icon,
    string? ImageUrl,
    bool IsActive
);
