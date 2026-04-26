namespace E_Club.DTOs.Coaches.Requests;

public record CreateCoachRequest(
    string FullName,
    string Specialization,
    string? ImageUrl,
    string? Bio,
    int ExperienceYears,
    string? PhoneNumber,
    string? Email
);
