namespace E_Club.Services;

public class CoachService(ApplicationDbContext context) : ICoachService
{
    public async Task<Result<IEnumerable<CoachResponse>>> GetAllCoachesAsync(CancellationToken cancellationToken = default)
    {
        var coaches = await context.Coaches
            .Where(c => c.IsActive)
            .OrderBy(c => c.FullName)
            .Select(c => MapToResponse(c))
            .ToListAsync(cancellationToken);

        return Result.Success(coaches.AsEnumerable());
    }

    public async Task<Result<CoachResponse>> GetCoachByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var coach = await context.Coaches.FindAsync([id], cancellationToken);
        if (coach is null)
            return Result.Failure<CoachResponse>(CoachErrors.CoachNotFound);

        return Result.Success(MapToResponse(coach));
    }

    public async Task<Result<IEnumerable<CoachResponse>>> GetCoachesBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        var coaches = await context.Coaches
            .Where(c => c.IsActive && c.Specialization.ToLower() == specialization.ToLower())
            .OrderByDescending(c => c.Rating)
            .Select(c => MapToResponse(c))
            .ToListAsync(cancellationToken);

        return Result.Success(coaches.AsEnumerable());
    }

    public async Task<Result<CoachResponse>> CreateCoachAsync(CreateCoachRequest request, CancellationToken cancellationToken = default)
    {
        var coach = new Coach
        {
            FullName = request.FullName,
            Specialization = request.Specialization,
            ImageUrl = request.ImageUrl,
            Bio = request.Bio,
            ExperienceYears = request.ExperienceYears,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            IsActive = true
        };

        context.Coaches.Add(coach);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(MapToResponse(coach));
    }

    public async Task<Result> UpdateCoachAsync(int id, CreateCoachRequest request, CancellationToken cancellationToken = default)
    {
        var coach = await context.Coaches.FindAsync([id], cancellationToken);
        if (coach is null)
            return Result.Failure(CoachErrors.CoachNotFound);

        coach.FullName = request.FullName;
        coach.Specialization = request.Specialization;
        coach.ImageUrl = request.ImageUrl;
        coach.Bio = request.Bio;
        coach.ExperienceYears = request.ExperienceYears;
        coach.PhoneNumber = request.PhoneNumber;
        coach.Email = request.Email;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteCoachAsync(int id, CancellationToken cancellationToken = default)
    {
        var coach = await context.Coaches.FindAsync([id], cancellationToken);
        if (coach is null)
            return Result.Failure(CoachErrors.CoachNotFound);

        // Soft delete
        coach.IsActive = false;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> AssignCoachToClassAsync(int classId, int coachId, CancellationToken cancellationToken = default)
    {
        var sportClass = await context.SportClasses.FindAsync([classId], cancellationToken);
        if (sportClass is null)
            return Result.Failure(SportErrors.ClassNotFound);

        var coach = await context.Coaches.FindAsync([coachId], cancellationToken);
        if (coach is null)
            return Result.Failure(CoachErrors.CoachNotFound);

        sportClass.CoachId = coachId;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RemoveCoachFromClassAsync(int classId, CancellationToken cancellationToken = default)
    {
        var sportClass = await context.SportClasses.FindAsync([classId], cancellationToken);
        if (sportClass is null)
            return Result.Failure(SportErrors.ClassNotFound);

        sportClass.CoachId = null;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private static CoachResponse MapToResponse(Coach c) => new(
        c.Id, c.FullName, c.Specialization, c.ImageUrl,
        c.Bio, c.ExperienceYears, c.Rating,
        c.PhoneNumber, c.Email, c.IsActive
    );
}
