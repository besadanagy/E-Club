namespace E_Club.Controllers;

[Route("api/notifications")]
[ApiController]
[Authorize]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    /// <summary>
    /// Get current user's notifications (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await notificationService.GetUserNotificationsAsync(userId, page, pageSize, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Get unread notifications count
    /// </summary>
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await notificationService.GetUnreadCountAsync(userId, cancellationToken);
        return result.IsSuccess ? Ok(new { count = result.Value }) : result.ToProblem();
    }

    /// <summary>
    /// Mark a specific notification as read
    /// </summary>
    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await notificationService.MarkAsReadAsync(userId, id, cancellationToken);
        return result.IsSuccess ? Ok(new { message = "Notification marked as read" }) : result.ToProblem();
    }

    /// <summary>
    /// Mark all notifications as read
    /// </summary>
    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await notificationService.MarkAllAsReadAsync(userId, cancellationToken);
        return result.IsSuccess ? Ok(new { message = "All notifications marked as read" }) : result.ToProblem();
    }

    /// <summary>
    /// Create/Send a notification (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateNotification(
        [FromBody] SendNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await notificationService.CreateNotificationAsync(request, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetMyNotifications), result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Delete a notification (Admin only)
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteNotification(int id, CancellationToken cancellationToken)
    {
        var result = await notificationService.DeleteNotificationAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(new { message = "Notification deleted successfully" }) : result.ToProblem();
    }
}
