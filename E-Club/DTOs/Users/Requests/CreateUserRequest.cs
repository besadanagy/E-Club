namespace E_Club.DTOs.Users.Requests;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    IList<string> Roles
);