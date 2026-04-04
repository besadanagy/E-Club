
namespace E_Club.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EventService> _logger;

    public EventService(
        ApplicationDbContext context,
        ILogger<EventService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<EventResponse>> GetFeaturedEventAsync()
    {
        try
        {
            var featuredEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.IsFeatured && e.Status == EventStatus.Upcoming);

            if (featuredEvent == null)
                return Result.Failure<EventResponse>(
                    new Error("Event.NotFound", "No featured event found", 404)
                );

            var response = MapToResponse(featuredEvent);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting featured event");
            return Result.Failure<EventResponse>(
                new Error("Event.Error", "An error occurred while fetching the event", 500)
            );
        }
    }

    public async Task<Result<IEnumerable<EventResponse>>> GetUpcomingEventsAsync()
    {
        try
        {
            var events = await _context.Events
                .Where(e => e.StartDate > DateTime.UtcNow && e.Status == EventStatus.Upcoming)
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            var response = events.Select(MapToResponse);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming events");
            return Result.Failure<IEnumerable<EventResponse>>(
                new Error("Event.Error", "An error occurred while fetching events", 500)
            );
        }
    }

    public async Task<Result<EventResponse>> GetEventByIdAsync(int id)
    {
        try
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return Result.Failure<EventResponse>(
                    new Error("Event.NotFound", "Event not found", 404)
                );

            var response = MapToResponse(@event);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event {EventId}", id);
            return Result.Failure<EventResponse>(
                new Error("Event.Error", "An error occurred while fetching the event", 500)
            );
        }
    }

    public async Task<Result<EventResponse>> CreateEventAsync(CreateEventRequest request, string userId)
    {
        try
        {
            var @event = new Event
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ImageUrl = request.ImageUrl,
                MaxParticipants = request.MaxParticipants,
                CurrentParticipants = 0,
                IsFeatured = false,
                Status = EventStatus.Upcoming,
                CreatedById = userId,
                CreatedOn = DateTime.UtcNow
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            var response = MapToResponse(@event);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event. Details: {Message}, InnerException: {InnerException}, StackTrace: {StackTrace}",
                ex.Message, ex.InnerException?.Message, ex.StackTrace);
            return Result.Failure<EventResponse>(
                new Error("Event.Error", $"An error occurred while creating the event: {ex.Message}", 500)
            );
        }
    }

    public async Task<Result> UpdateEventAsync(int id, CreateEventRequest request)
    {
        try
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return Result.Failure(
                    new Error("Event.NotFound", "Event not found", 404)
                );

            @event.Title = request.Title;
            @event.Description = request.Description;
            @event.Location = request.Location;
            @event.StartDate = request.StartDate;
            @event.EndDate = request.EndDate;
            @event.ImageUrl = request.ImageUrl;
            @event.MaxParticipants = request.MaxParticipants;
            @event.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", id);
            return Result.Failure(
                new Error("Event.Error", "An error occurred while updating the event", 500)
            );
        }
    }

    public async Task<Result> DeleteEventAsync(int id)
    {
        try
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
                return Result.Failure(
                    new Error("Event.NotFound", "Event not found", 404)
                );

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", id);
            return Result.Failure(
                new Error("Event.Error", "An error occurred while deleting the event", 500)
            );
        }
    }

    private EventResponse MapToResponse(Event @event)
    {
        var now = DateTime.UtcNow;
        var timeLeft = @event.StartDate - now;

        var countdown = new CountdownResponse(
            Days: Math.Max(0, timeLeft.Days),
            Hours: Math.Max(0, timeLeft.Hours),
            Minutes: Math.Max(0, timeLeft.Minutes),
            Seconds: Math.Max(0, timeLeft.Seconds)
        );

        return new EventResponse(
            Id: @event.Id,
            Title: @event.Title,
            Description: @event.Description,
            Location: @event.Location,
            StartDate: @event.StartDate,
            EndDate: @event.EndDate,
            ImageUrl: @event.ImageUrl,
            MaxParticipants: @event.MaxParticipants,
            CurrentParticipants: @event.CurrentParticipants,
            Status: @event.Status.ToString(),
            Countdown: @event.StartDate > now ? countdown : null
        );
    }
}