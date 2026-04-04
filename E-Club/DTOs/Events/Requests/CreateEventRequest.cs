namespace E_Club.DTOs.Events.Requests;

public record CreateEventRequest(
    string Title,
    string Description,
    string Location,
    DateTime StartDate,
    DateTime EndDate,
    string? ImageUrl,
    int MaxParticipants
);