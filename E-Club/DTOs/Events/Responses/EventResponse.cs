namespace E_Club.DTOs.Events.Responses;

public record EventResponse(
    int Id,
    string Title,
    string Description,
    string Location,
    DateTime StartDate,
    DateTime EndDate,
    string? ImageUrl,
    int MaxParticipants,
    int CurrentParticipants,
    string Status,
    CountdownResponse? Countdown
);