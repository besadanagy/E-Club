
namespace E_Club.Interfaces;

public interface IEventService
{
    // للكل (محتاج Authentication)
    Task<Result<EventResponse>> GetFeaturedEventAsync();
    Task<Result<IEnumerable<EventResponse>>> GetUpcomingEventsAsync();
    Task<Result<EventResponse>> GetEventByIdAsync(int id);

    // للإدمن فقط
    Task<Result<EventResponse>> CreateEventAsync(CreateEventRequest request, string userId);
    Task<Result> UpdateEventAsync(int id, CreateEventRequest request);
    Task<Result> DeleteEventAsync(int id);
}