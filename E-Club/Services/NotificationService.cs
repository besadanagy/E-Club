namespace E_Club.Services;

public class NotificationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : INotificationService
{
    public async Task<Result<List<NotificationResponse>>> GetUserNotificationsAsync(
        string userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var notifications = await context.Notifications
            .Where(n => n.UserId == userId || n.UserId == null) // user-specific + broadcast
            .OrderByDescending(n => n.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationResponse(
                n.Id,
                n.Title,
                n.Body,
                n.Type.ToString(),
                n.IsRead,
                n.ReadAt,
                n.ReferenceId,
                n.ReferenceType,
                n.CreatedOn
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(notifications);
    }

    public async Task<Result<int>> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        var count = await context.Notifications
            .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
            .CountAsync(cancellationToken);

        return Result.Success(count);
    }

    public async Task<Result> MarkAsReadAsync(string userId, int notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && (n.UserId == userId || n.UserId == null), cancellationToken);

        if (notification is null)
            return Result.Failure(NotificationErrors.NotificationNotFound);

        if (notification.IsRead)
            return Result.Success(); // already read, no-op

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
    {
        var unreadNotifications = await context.Notifications
            .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<NotificationResponse>> CreateNotificationAsync(
        SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        // If UserId is specified, verify user exists
        if (!string.IsNullOrEmpty(request.UserId))
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result.Failure<NotificationResponse>(UserErrors.UserNotFound);
        }

        var notification = new Notification
        {
            Title = request.Title,
            Body = request.Body,
            Type = request.Type,
            UserId = request.UserId,
            ReferenceId = request.ReferenceId,
            ReferenceType = request.ReferenceType,
            CreatedOn = DateTime.UtcNow
        };

        context.Notifications.Add(notification);
        await context.SaveChangesAsync(cancellationToken);

        var response = new NotificationResponse(
            notification.Id,
            notification.Title,
            notification.Body,
            notification.Type.ToString(),
            notification.IsRead,
            notification.ReadAt,
            notification.ReferenceId,
            notification.ReferenceType,
            notification.CreatedOn
        );

        return Result.Success(response);
    }

    public async Task<Result> DeleteNotificationAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

        if (notification is null)
            return Result.Failure(NotificationErrors.NotificationNotFound);

        context.Notifications.Remove(notification);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
