namespace E_Club.DTOs.Coaches.Responses;

public record CoachResponse(
    int Id,
    string FullName,
    string Specialization,
    string? ImageUrl,
    string? Bio,
    int ExperienceYears,
    double Rating,
    string? PhoneNumber,
    string? Email,
    bool IsActive
);
