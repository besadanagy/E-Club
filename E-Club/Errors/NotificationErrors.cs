namespace E_Club.Errors;

public static class NotificationErrors
{
    public static readonly Error NotificationNotFound =
        new("Notification.NotFound", "Notification not found", StatusCodes.Status404NotFound);

    public static readonly Error NotificationAlreadyRead =
        new("Notification.AlreadyRead", "Notification is already marked as read", StatusCodes.Status400BadRequest);
}
