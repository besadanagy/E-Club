namespace E_Club.Services;

public class EventRegistrationService(ApplicationDbContext context) : IEventRegistrationService
{
    public async Task<Result<EventRegistrationResponse>> RegisterAsync(int eventId, string userId)
    {
        var @event = await context.Events.FindAsync(eventId);

        if (@event is null)
            return Result.Failure<EventRegistrationResponse>(EventRegistrationErrors.EventNotFound);

        if (@event.Status != EventStatus.Upcoming)
            return Result.Failure<EventRegistrationResponse>(EventRegistrationErrors.EventClosed);

        if (@event.CurrentParticipants >= @event.MaxParticipants)
            return Result.Failure<EventRegistrationResponse>(EventRegistrationErrors.EventFull);

        var exists = await context.EventRegistrations
            .AnyAsync(r => r.EventId == eventId && r.UserId == userId);

        if (exists)
            return Result.Failure<EventRegistrationResponse>(EventRegistrationErrors.AlreadyRegistered);

        var registration = new EventRegistration
        {
            EventId = eventId,
            UserId = userId,
            RegisteredOn = DateTime.UtcNow,
            Status = RegistrationStatus.Confirmed
        };

        context.EventRegistrations.Add(registration);
        @event.CurrentParticipants++;
        await context.SaveChangesAsync();

        return Result.Success(new EventRegistrationResponse(
            registration.Id,
            @event.Id,
            @event.Title,
            registration.RegisteredOn,
            registration.Status.ToString()
        ));
    }

    public async Task<Result> CancelRegistrationAsync(int eventId, string userId)
    {
        var registration = await context.EventRegistrations
            .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

        if (registration is null)
            return Result.Failure(EventRegistrationErrors.RegistrationNotFound);

        if (registration.Status == RegistrationStatus.Cancelled)
            return Result.Failure(EventRegistrationErrors.AlreadyCancelled);

        registration.Status = RegistrationStatus.Cancelled;

        var @event = await context.Events.FindAsync(eventId);
        if (@event is not null && @event.CurrentParticipants > 0)
            @event.CurrentParticipants--;

        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<EventRegistrationResponse>>> GetMyRegistrationsAsync(string userId)
    {
        var registrations = await context.EventRegistrations
            .Include(r => r.Event)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RegisteredOn)
            .Select(r => new EventRegistrationResponse(
                r.Id, r.EventId, r.Event.Title, r.RegisteredOn, r.Status.ToString()))
            .ToListAsync();

        return Result.Success(registrations.AsEnumerable());
    }
}

public static class EventRegistrationErrors
{
    public static readonly Error EventNotFound = new("Event.NotFound", "Event not found", 404);
    public static readonly Error EventClosed = new("Event.Closed", "Event is no longer accepting registrations", 400);
    public static readonly Error EventFull = new("Event.Full", "Event is fully booked", 400);
    public static readonly Error AlreadyRegistered = new("Event.AlreadyRegistered", "Already registered for this event", 409);
    public static readonly Error RegistrationNotFound = new("Registration.NotFound", "Registration not found", 404);
    public static readonly Error AlreadyCancelled = new("Registration.AlreadyCancelled", "Registration already cancelled", 400);
}
