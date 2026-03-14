namespace E_Club.DTOs.Roles;

public record UserInRoleResponse(
    string UserId,
    string UserName,
    string Email,
    bool IsInRole
);