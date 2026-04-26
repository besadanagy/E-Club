namespace E_Club.Interfaces;

public interface ICoachService
{
    Task<Result<IEnumerable<CoachResponse>>> GetAllCoachesAsync(CancellationToken cancellationToken = default);
    Task<Result<CoachResponse>> GetCoachByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CoachResponse>>> GetCoachesBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
    Task<Result<CoachResponse>> CreateCoachAsync(CreateCoachRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateCoachAsync(int id, CreateCoachRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteCoachAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> AssignCoachToClassAsync(int classId, int coachId, CancellationToken cancellationToken = default);
    Task<Result> RemoveCoachFromClassAsync(int classId, CancellationToken cancellationToken = default);
}
