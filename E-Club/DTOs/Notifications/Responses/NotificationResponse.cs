namespace E_Club.DTOs.Notifications.Responses;

public record NotificationResponse(
    int Id,
    string Title,
    string Body,
    string Type,
    bool IsRead,
    DateTime? ReadAt,
    int? ReferenceId,
    string? ReferenceType,
    DateTime CreatedOn
);
