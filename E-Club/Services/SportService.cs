namespace E_Club.Services;

public class SportService(ApplicationDbContext context, ILogger<SportService> logger) : ISportService
{
    public async Task<Result<SportsScreenResponse>> GetSportsScreenAsync(int? sportId, string userId)
    {
        var sports = await GetSportsListAsync();
        var activeSportId = sportId ?? sports.FirstOrDefault()?.Id;

        var classes = await GetClassesAsync(activeSportId, userId, ClassType.Regular, null, null);
        var specialList = await GetClassesAsync(activeSportId, userId, ClassType.Special, null, null);

        return Result.Success(new SportsScreenResponse(sports, classes, specialList.FirstOrDefault()));
    }

    public async Task<Result<IEnumerable<SportResponse>>> GetAllSportsAsync()
    {
        var sports = await GetSportsListAsync();
        return Result.Success(sports.AsEnumerable());
    }

    public async Task<Result<IEnumerable<SportClassResponse>>> GetUpcomingClassesAsync(int? sportId, string userId, int? coachId = null, DateTime? date = null)
    {
        var classes = await GetClassesAsync(sportId, userId, ClassType.Regular, coachId, date);
        return Result.Success(classes.AsEnumerable());
    }

    public async Task<Result<SportClassResponse>> GetSpecialEventAsync(int? sportId, string userId)
    {
        var specials = await GetClassesAsync(sportId, userId, ClassType.Special, null, null);
        var special = specials.FirstOrDefault();

        return special is null
            ? Result.Failure<SportClassResponse>(SportErrors.NoSpecialEvent)
            : Result.Success(special);
    }

    public async Task<Result<ClassBookingResponse>> BookClassAsync(int classId, string userId)
    {
        var sportClass = await context.SportClasses
            .Include(c => c.Sport)
            .FirstOrDefaultAsync(c => c.Id == classId);

        if (sportClass is null)
            return Result.Failure<ClassBookingResponse>(SportErrors.ClassNotFound);

        if (sportClass.Status != ClassStatus.Upcoming)
            return Result.Failure<ClassBookingResponse>(SportErrors.ClassClosed);

        if (sportClass.CurrentParticipants >= sportClass.MaxParticipants)
            return Result.Failure<ClassBookingResponse>(SportErrors.ClassFull);

        var alreadyBooked = await context.ClassBookings
            .AnyAsync(b => b.SportClassId == classId && b.UserId == userId);

        if (alreadyBooked)
            return Result.Failure<ClassBookingResponse>(SportErrors.AlreadyBooked);

        var booking = new ClassBooking
        {
            SportClassId = classId,
            UserId = userId,
            BookedOn = DateTime.UtcNow,
            Status = BookingStatus.Confirmed
        };

        context.ClassBookings.Add(booking);
        sportClass.CurrentParticipants++;
        await context.SaveChangesAsync();

        return Result.Success(new ClassBookingResponse(
            booking.Id,
            sportClass.Id,
            sportClass.Title,
            sportClass.Sport.Name,
            $"{sportClass.StartTime:hh:mm tt} - {sportClass.EndTime:hh:mm tt}",
            sportClass.Location,
            booking.BookedOn,
            booking.Status.ToString()
        ));
    }

    public async Task<Result> CancelBookingAsync(int classId, string userId)
    {
        var booking = await context.ClassBookings
            .FirstOrDefaultAsync(b => b.SportClassId == classId && b.UserId == userId);

        if (booking is null)
            return Result.Failure(SportErrors.BookingNotFound);

        if (booking.Status == BookingStatus.Cancelled)
            return Result.Failure(SportErrors.AlreadyCancelled);

        booking.Status = BookingStatus.Cancelled;

        var sportClass = await context.SportClasses.FindAsync(classId);
        if (sportClass is not null && sportClass.CurrentParticipants > 0)
            sportClass.CurrentParticipants--;

        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IEnumerable<ClassBookingResponse>>> GetMyBookingsAsync(string userId)
    {
        var bookings = await context.ClassBookings
            .Include(b => b.SportClass).ThenInclude(c => c.Sport)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookedOn)
            .Select(b => new ClassBookingResponse(
                b.Id,
                b.SportClassId,
                b.SportClass.Title,
                b.SportClass.Sport.Name,
                $"{b.SportClass.StartTime:hh:mm tt} - {b.SportClass.EndTime:hh:mm tt}",
                b.SportClass.Location,
                b.BookedOn,
                b.Status.ToString()))
            .ToListAsync();

        return Result.Success(bookings.AsEnumerable());
    }

    public async Task<Result<SportClassResponse>> CreateClassAsync(CreateSportClassRequest request, string userId)
    {
        var sport = await context.Sports.FindAsync(request.SportId);
        if (sport is null)
            return Result.Failure<SportClassResponse>(SportErrors.SportNotFound);

        if (!Enum.TryParse<ClassType>(request.Type, out var classType))
            return Result.Failure<SportClassResponse>(SportErrors.InvalidType);

        var sportClass = new SportClass
        {
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            SportId = request.SportId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Location = request.Location,
            MaxParticipants = request.MaxParticipants,
            Price = request.Price,
            Type = classType,
            Status = ClassStatus.Upcoming,
            CoachId = request.CoachId,
            CreatedById = userId,
            CreatedOn = DateTime.UtcNow
        };

        context.SportClasses.Add(sportClass);
        await context.SaveChangesAsync();

        return Result.Success(MapClass(sportClass, sport.Name, false));
    }

    public async Task<Result> UpdateClassAsync(int id, CreateSportClassRequest request)
    {
        var sportClass = await context.SportClasses.FindAsync(id);
        if (sportClass is null) return Result.Failure(SportErrors.ClassNotFound);

        if (!Enum.TryParse<ClassType>(request.Type, out var classType))
            return Result.Failure(SportErrors.InvalidType);

        sportClass.Title = request.Title;
        sportClass.Description = request.Description;
        sportClass.ImageUrl = request.ImageUrl;
        sportClass.SportId = request.SportId;
        sportClass.StartTime = request.StartTime;
        sportClass.EndTime = request.EndTime;
        sportClass.Location = request.Location;
        sportClass.MaxParticipants = request.MaxParticipants;
        sportClass.Price = request.Price;
        sportClass.Type = classType;
        sportClass.UpdatedOn = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteClassAsync(int id)
    {
        var sportClass = await context.SportClasses.FindAsync(id);
        if (sportClass is null) return Result.Failure(SportErrors.ClassNotFound);

        context.SportClasses.Remove(sportClass);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    // ── Private Helpers ─────────────────────────────────────

    private async Task<List<SportResponse>> GetSportsListAsync() =>
        await context.Sports
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SportResponse(s.Id, s.Name, s.Icon, s.ImageUrl, s.IsActive))
            .ToListAsync();

    private async Task<List<SportClassResponse>> GetClassesAsync(int? sportId, string userId, ClassType type, int? coachId, DateTime? date)
    {
        var myBookings = await context.ClassBookings
            .Where(b => b.UserId == userId && b.Status == BookingStatus.Confirmed)
            .Select(b => b.SportClassId)
            .ToListAsync();

        var query = context.SportClasses
            .Include(c => c.Sport)
            .Include(c => c.Coach)
            .Where(c => c.Type == type
                     && c.Status == ClassStatus.Upcoming
                     && c.StartTime > DateTime.UtcNow);

        if (sportId.HasValue)
            query = query.Where(c => c.SportId == sportId.Value);

        if (coachId.HasValue)
            query = query.Where(c => c.CoachId == coachId.Value);

        if (date.HasValue)
        {
            var targetDate = date.Value.Date;
            query = query.Where(c => c.StartTime.Date == targetDate);
        }

        var classes = await query.OrderBy(c => c.StartTime).ToListAsync();

        return classes.Select(c => MapClass(c, c.Sport.Name, myBookings.Contains(c.Id))).ToList();
    }

    private static SportClassResponse MapClass(SportClass c, string sportName, bool isBooked)
    {
        var timeLeft = c.StartTime - DateTime.UtcNow;
        var countdown = c.Type == ClassType.Special && timeLeft.TotalSeconds > 0
            ? new CountdownResponse(
                Math.Max(0, timeLeft.Days),
                Math.Max(0, timeLeft.Hours),
                Math.Max(0, timeLeft.Minutes),
                Math.Max(0, timeLeft.Seconds))
            : null;

        return new SportClassResponse(
            c.Id,
            c.Title,
            c.Description,
            c.ImageUrl,
            sportName,
            c.StartTime.ToString("hh:mm tt"),      // StartTime
            c.EndTime.ToString("hh:mm tt"),        // EndTime
            $"{c.StartTime:hh:mm tt} - {c.EndTime:hh:mm tt}", // TimeRange
            c.Location,
            c.MaxParticipants,
            c.CurrentParticipants,
            c.MaxParticipants - c.CurrentParticipants,
            c.Status.ToString(),
            c.Type.ToString(),
            c.Price,
            isBooked,
            countdown,
            c.CoachId,
            c.Coach?.FullName,
            c.Coach?.ImageUrl
        );
    }
}

