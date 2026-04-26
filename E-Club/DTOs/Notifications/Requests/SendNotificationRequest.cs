namespace E_Club.DTOs.Notifications.Requests;

public record SendNotificationRequest(
    string Title,
    string Body,
    NotificationType Type,
    string? UserId,          // null = broadcast to all users
    int? ReferenceId,
    string? ReferenceType
);
