namespace E_Club.DTOs.Users.Responses;

public record UserProfileResponse(
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? NationalId,
    string? MembershipId,
    string? SequenceNumber,
    string? DigitalAccessKey,
    string? ClubCode,
    DateTime CreatedOn,
    IList<string> Roles
);