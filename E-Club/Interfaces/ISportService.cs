namespace E_Club.Interfaces;

public interface ISportService
{
    Task<Result<SportsScreenResponse>> GetSportsScreenAsync(int? sportId, string userId);
    Task<Result<IEnumerable<SportResponse>>> GetAllSportsAsync();
    Task<Result<IEnumerable<SportClassResponse>>> GetUpcomingClassesAsync(int? sportId, string userId, int? coachId = null, DateTime? date = null);
    Task<Result<SportClassResponse>> GetSpecialEventAsync(int? sportId, string userId);
    Task<Result<ClassBookingResponse>> BookClassAsync(int classId, string userId);
    Task<Result> CancelBookingAsync(int classId, string userId);
    Task<Result<IEnumerable<ClassBookingResponse>>> GetMyBookingsAsync(string userId);
    Task<Result<SportClassResponse>> CreateClassAsync(CreateSportClassRequest request, string userId);
    Task<Result> UpdateClassAsync(int id, CreateSportClassRequest request);
    Task<Result> DeleteClassAsync(int id);
}
