namespace E_Club.DTOs.Sports.Requests;

public record CreateSportClassRequest(
    string Title,
    string? Description,
    string? ImageUrl,
    int SportId,
    DateTime StartTime,
    DateTime EndTime,
    string Location,
    int MaxParticipants,
    decimal Price,
    string Type
);
