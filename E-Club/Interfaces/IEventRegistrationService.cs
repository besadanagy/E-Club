namespace E_Club.Interfaces;

public interface IEventRegistrationService
{
    Task<Result<EventRegistrationResponse>> RegisterAsync(int eventId, string userId);
    Task<Result> CancelRegistrationAsync(int eventId, string userId);
    Task<Result<IEnumerable<EventRegistrationResponse>>> GetMyRegistrationsAsync(string userId);
}
