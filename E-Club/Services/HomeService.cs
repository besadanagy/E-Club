namespace E_Club.Services;

public class HomeService(ApplicationDbContext context, ILogger<HomeService> logger) : IHomeService
{
    public async Task<Result<HomeResponse>> GetHomeDataAsync(string? userId = null)
    {
        var banners = await context.Banners
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .Select(b => new BannerResponse(b.Id, b.Title, b.Subtitle, b.ImageUrl, b.ActionUrl, b.Type.ToString()))
            .ToListAsync();

        var services = await context.Services
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Take(4)
            .Select(s => new ServiceResponse(
                s.Id, s.Name, s.Description, s.Icon,
                s.Endpoint, s.ImageUrl, s.IsActive, s.DisplayOrder, s.Type.ToString()))
            .ToListAsync();

        var featuredEvent = await context.Events
            .Where(e => e.IsFeatured && e.Status == EventStatus.Upcoming)
            .FirstOrDefaultAsync();

        EventResponse? eventResponse = null;

        if (featuredEvent is not null)
        {
            var timeLeft = featuredEvent.StartDate - DateTime.UtcNow;
            var countdown = timeLeft.TotalSeconds > 0
                ? new CountdownResponse(
                    Math.Max(0, timeLeft.Days),
                    Math.Max(0, timeLeft.Hours),
                    Math.Max(0, timeLeft.Minutes),
                    Math.Max(0, timeLeft.Seconds))
                : null;

            eventResponse = new EventResponse(
                featuredEvent.Id,
                featuredEvent.Title,
                featuredEvent.Description,
                featuredEvent.Location,
                featuredEvent.StartDate,
                featuredEvent.EndDate,
                featuredEvent.ImageUrl,
                featuredEvent.MaxParticipants,
                featuredEvent.CurrentParticipants,
                featuredEvent.Status.ToString(),
                countdown
            );
        }

        // Unread notifications count
        int unreadCount = 0;
        if (!string.IsNullOrEmpty(userId))
        {
            unreadCount = await context.Notifications
                .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
                .CountAsync();
        }

        return Result.Success(new HomeResponse(banners, services, eventResponse, unreadCount));
    }
}

