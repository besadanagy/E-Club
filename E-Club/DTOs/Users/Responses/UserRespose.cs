namespace E_Club.DTOs.Users.Responses;

public record UserResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsDisabled,
    IEnumerable<string> Roles
);