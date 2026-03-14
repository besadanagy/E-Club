namespace E_Club.DTOs.Roles;

public record RoleRequest(
    string Name,
    IEnumerable<string>? Permissions = null,
    IEnumerable<string>? UsersIds = null
);