namespace E_Club.Interfaces;

public interface INotificationService
{
    Task<Result<List<NotificationResponse>>> GetUserNotificationsAsync(string userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<Result<int>> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> MarkAsReadAsync(string userId, int notificationId, CancellationToken cancellationToken = default);
    Task<Result> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<NotificationResponse>> CreateNotificationAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteNotificationAsync(int id, CancellationToken cancellationToken = default);
}
