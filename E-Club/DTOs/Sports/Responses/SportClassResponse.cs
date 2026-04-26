
namespace E_Club.DTOs.Sports.Responses;

public record SportClassResponse(
    int Id,
    string Title,
    string? Description,
    string? ImageUrl,
    string SportName,
    string StartTime,
    string EndTime,
    string TimeRange,
    string Location,
    int MaxParticipants,
    int CurrentParticipants,
    int AvailableSlots,
    string Status,
    string Type,
    decimal Price,
    bool IsBookedByCurrentUser,
    CountdownResponse? Countdown,
    int? CoachId,
    string? CoachName,
    string? CoachImageUrl
);
