namespace E_Club.DTOs.Auth.Responses;

public record AuthResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration,
    string? MembershipId = null,
    string? DigitalAccessKey = null
);